using Esd.FlexibleOpenGeographies.Dtos;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaBasicWithTypeForTypeAndCode : IQuerySingle<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly string _code;

        public AreaBasicWithTypeForTypeAndCode(IContextFactory contextFactory, string typeCode, string code)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _code = code;
        }

        public AreaBasicWithType Find()
        {
            using (var context = _contextFactory.Create())
            {
                return context.AreaDetails.AsNoTracking()
                              .Where(area => area.Code == _code && area.TypeCode == _typeCode)
                              .Select(area => new AreaBasicWithType
                              {
                                  Id = area.Id,
                                  Code = area.Code,
                                  Label = area.Label,
                                  TypeCode = area.AreaType.Code,
                                  TypeName = area.AreaType.Label
                              }).SingleOrDefault();
            }
        }
    }
}
