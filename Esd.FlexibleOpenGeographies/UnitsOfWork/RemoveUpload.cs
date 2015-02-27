using Esd.FlexibleOpenGeographies.Dtos;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class RemoveUpload : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly UploadBasic _upload;

        public RemoveUpload(IContextFactory contextFactory, UploadBasic upload)
        {
            _contextFactory = contextFactory;
            _upload = upload;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var tmp = context.Uploads.SingleOrDefault(u => u.Id == _upload.ID);
                context.Uploads.Remove(tmp);
                context.SaveChanges();
            }
        }
    }
}
