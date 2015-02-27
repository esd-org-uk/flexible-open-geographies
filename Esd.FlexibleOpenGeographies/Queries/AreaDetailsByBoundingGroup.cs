using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaDetailsByBoundingGroup : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private int _areaId;
        private string _areaTypeCode;

        public AreaDetailsByBoundingGroup(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForAreaId(int id)
        {
            _areaId = id;
        }

        public void ForAreaTypeCode(string code)
        {
            _areaTypeCode = code;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            using (var context = _contextFactory.Create())
            {
                var areaIds = new List<int>();

                if (!string.IsNullOrEmpty(_areaTypeCode))
                {
                    var query = new ChildAreasForAreaAndAreaType(_contextFactory);
                    query.ForAreaId(_areaId);
                    query.ForTypeCode(_areaTypeCode);
                    var areas = query.Fetch();

                    if (areas != null)
                    {
                        areaIds.AddRange(areas.Select(area => area.Id));
                    }
                }
                else
                {
                    areaIds.Add(_areaId);
                }

                return context.AreaDetails.AsNoTracking().Where(area => areaIds.Contains(area.Id)).Select(area => new AreaBasicWithType
                              {
                                  Code = area.Code,
                                  Label = area.Label,
                                  TypeCode = area.AreaType.Code,
                                  TypeName = area.AreaType.Label
                              }).ToList();
            }
        }
    }
}
