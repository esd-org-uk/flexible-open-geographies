using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class AddUpload : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly UploadBasic _upload;

        public AddUpload(IContextFactory contextFactory, UploadBasic upload)
        {
            _contextFactory = contextFactory;
            _upload = upload;
        }

        public void Execute()
        {
            var upload = UploadMapper.MapUpload(_upload);
            using (var context = _contextFactory.Create())
            {
                context.Uploads.Add(upload);
                context.SaveChanges();
            }
        }
    }
}
