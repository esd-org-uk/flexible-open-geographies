using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ChildAreasForAreaAndAreaType : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private int _areaId;
        private string _typeCode;

        public ChildAreasForAreaAndAreaType(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForAreaId(int areaId)
        {
            _areaId = areaId;
        }

        public void ForTypeCode(string typeCode)
        {
            _typeCode = typeCode;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            var results = new List<AreaBasicWithType>();
            using (var context = _contextFactory.Create())
            {
                results.AddRange(context.AreaCompositions.Include(composition => composition.ChildArea.AreaType).AsNoTracking()
                              .Where(composition => composition.Area.Id == _areaId && composition.ChildArea.AreaType.Code == _typeCode)
                              .Select(composition => composition.ChildArea)
                    //Can't get this working with mapper
                              .Select(area => new AreaBasicWithType
                              {
                                  Id = area.Id,
                                  Code = area.Code,
                                  Label = area.Label,
                                  TypeCode = area.AreaType.Code,
                                  TypeName = area.AreaType.Label
                              })
                              .ToList());


                var parentArea = context.AreaDetails.AsNoTracking()
                              .Where(a => a.Id == _areaId && a.AreaType.Code == _typeCode)
                              .Select(area => new AreaBasicWithType
                              {
                                  Id = area.Id,
                                  Code = area.Code,
                                  Label = area.Label,
                                  TypeCode = area.AreaType.Code,
                                  TypeName = area.AreaType.Label
                              }).SingleOrDefault();

                if (parentArea != null)
                {
                    results.Add(parentArea);
                }
            }

            return results;
        }
    }
}
