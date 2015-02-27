using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class HierarchicalAreaByIdAndType : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private int _areaId;
        private string _areaType;

        public HierarchicalAreaByIdAndType(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForAreaId(int areaId)
        {
            _areaId = areaId;
        }

        public void ForTypeCode(string areaType)
        {
            _areaType = areaType;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            List<int> ids = new List<int>();
            ids.Add(_areaId);

            return GetAllHierarchies(ids, _areaType);
        }

        private List<AreaBasicWithType> GetAllHierarchies(List<int> ids, string targetType)
        {
            var results = new List<AreaBasicWithType>();

            using (var context = _contextFactory.Create())
            {
                var children = context.AreaCompositions.Include(c => c.ChildArea.AreaType).AsNoTracking()
                              .Where(c => ids.Contains(c.AreaId))
                              .Select(c => c.ChildArea)
                              .Select(area => new AreaBasicWithType
                              {
                                  Id = area.Id,
                                  Code = area.Code,
                                  Label = area.Label,
                                  TypeCode = area.AreaType.Code,
                                  TypeName = area.AreaType.Label
                              }).ToList();

                ids.Clear();

                foreach(AreaBasicWithType child in children)
                {
                    ids.Add(child.Id);
                    if(child.TypeCode == targetType)
                    {
                        results.Add(child);
                    }
                }
            }

            if (results.Count == 0 && ids.Count > 0)
            {
                results.AddRange(GetAllHierarchies(ids, targetType));
            }

            return results;
        }
    }
}
