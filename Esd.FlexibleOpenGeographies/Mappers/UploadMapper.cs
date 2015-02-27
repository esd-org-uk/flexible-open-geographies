using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public class UploadMapper
    {
        public static UploadBasic MapBasic(Upload upload)
        {
            return new UploadBasic
            {
                ID = upload.Id,
                CSV = upload.CSV,
                UserId = upload.UserId
            };
        }

        public static Upload MapUpload(UploadBasic upload)
        {
            return new Upload
            {
                Id = upload.ID,
                CSV = upload.CSV,
                UserId = upload.UserId
            };
        }
    }
}
