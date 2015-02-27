using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class UploadBasicSingle : IQuerySingle<UploadBasic>
    {
        private readonly IContextFactory _contextFactory;

        public UploadBasicSingle(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public UploadBasic Find()
        {
            using (var context = _contextFactory.Create())
                return context.Uploads.AsNoTracking()
                              .OrderBy(upload => upload.Id)
                              .Select(UploadMapper.MapBasic)
                              .FirstOrDefault();
        }
    }
}
