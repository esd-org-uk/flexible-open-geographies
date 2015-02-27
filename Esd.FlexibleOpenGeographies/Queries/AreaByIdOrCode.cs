using Esd.FlexibleOpenGeographies.Dtos;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaByIdOrCode : IQuerySingle<AreaBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _idOrCode;
        private readonly string _typeCode;

        public AreaByIdOrCode(IContextFactory contextFactory, string idOrCode, string typeCode)
        {
            _contextFactory = contextFactory;
            _idOrCode = idOrCode;
            _typeCode = typeCode;
        }

        public AreaBasic Find()
        {
            int id;
            int.TryParse(_idOrCode, out id);
            using (var context = _contextFactory.Create())
                return id > 0
                    ? context.AreaDetails.AsNoTracking()
                             .Where(x => (x.Id == id || x.Code == _idOrCode) && x.TypeCode == _typeCode)
                             .Select(x => new AreaBasic {Code = x.Code, Id = x.Id, Label = x.Label})
                             .FirstOrDefault()
                    : context.AreaDetails.AsNoTracking()
                             .Where(x => x.Code == _idOrCode)
                             .Select(x => new AreaBasic {Code = x.Code, Id = x.Id, Label = x.Label})
                             .FirstOrDefault();
        }
    }
}
