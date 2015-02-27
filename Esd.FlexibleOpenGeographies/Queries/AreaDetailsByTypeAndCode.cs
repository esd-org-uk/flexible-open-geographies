using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaDetailsByTypeAndCode : IQuerySingle<AreaDetailsNoGeography>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly string _code;

        public AreaDetailsByTypeAndCode(IContextFactory contextFactory, string typeCode, string code)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _code = code;
        }

        public AreaDetailsNoGeography Find()
        {
            using (var context = _contextFactory.Create())
            {
                var entity = context.AreaDetails.AsNoTracking().SingleOrDefault(area => area.Code == _code && area.TypeCode == _typeCode);
                return entity == null ? null : AreaMapper.MapDetails(entity);
            }
        }
    }
}
