using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaBasicForTypeAndTerm : IQueryEnumerable<AreaBasic>
    {
        private readonly IEnumerable<string> _oversizedTypes = new List<string>
        {
            "OutputArea",
            "LLSOA",
            "AdministrativeWard"
        };

        private readonly IEnumerable<string> _localAuthorityTypes = new List<string>
        {
            "Unitary",
            "District",
            "County"
        };
        private readonly IContextFactory _contextFactory;
        private string _typeCode;
        private string _term;

        public AreaBasicForTypeAndTerm(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForType(string typeCode)
        {
            _typeCode = typeCode;
        }

        public void ForTerm(string term)
        {
            _term = string.IsNullOrWhiteSpace(term) ? string.Empty : term.ToLower();
        }

        public IEnumerable<AreaBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
            {
                var areas = FilteredAreas(context);

                if (!_oversizedTypes.Contains(_typeCode)) return areas;
                var foundLa = false;
                while (!foundLa)
                {
                    var parentAreas = ParentAreas(context, areas);
                    areas = TryFindLocalAuthorities(areas, parentAreas, out foundLa);
                }
                return areas;
            }
        }

        private IList<AreaBasic> TryFindLocalAuthorities(IEnumerable<AreaBasic> areas, IList<AreaParentLink> parentAreas, out bool foundLa)
        {
            foundLa = false;
            var results = new List<AreaBasic>();
            foreach (var area in areas)
            {
                AreaBasic foundArea = null;
                foreach (var parent in parentAreas)
                {
                    foundLa = foundLa || _localAuthorityTypes.Contains(parent.ParentTypeID);
                    if (parent.ChildID == area.Id)
                        foundArea = new AreaBasic
                        {
                            Id = area.Id,
                            Code = area.Code,
                            Label = string.Format("{0}, {1}", area.Label, parent.ParentLabel)
                        };
                    if (foundArea != null) break;
                }

                results.Add(foundArea ?? area);
            }

            return results;
        }

        private static IList<AreaParentLink> ParentAreas(IFogContext context, IEnumerable<AreaBasic> areas)
        {
            var ids = areas.Select(area => area.Id).ToList();
            return context.AreaCompositions.AsNoTracking()
                                     .Where(composition => ids.Contains(composition.ChildAreaId))
                                     .Select(composition => new AreaParentLink
                                     {
                                         ParentID = composition.Area.Id,
                                         ParentLabel = composition.Area.Label,
                                         ParentTypeID = composition.Area.TypeCode,
                                         ChildID = composition.ChildAreaId
                                     })
                                     .ToList();
        }

        private IList<AreaBasic> FilteredAreas(IFogContext context)
        {
            return context.AreaDetails.AsNoTracking()
                               .Where(area => area.TypeCode == _typeCode &&
                                              (_term == string.Empty || area.Label.ToLower().Contains(_term)))
                               .OrderBy(area => area.Label)
                               .Take(1000)
                               .Select(AreaMapper.MapBasic)
                               .ToList();
        }

        private class AreaParentLink
        {
// used in Lambda. ReSharper doesn't detect this properly
// ReSharper disable UnusedAutoPropertyAccessor.Local
            public int ParentID {get;set;}
            public string ParentLabel { get; set; }
            public string ParentTypeID { get; set; }
            public int ChildID { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
