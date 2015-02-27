using Esd.FlexibleOpenGeographies.Data;
using Npgsql;

namespace Esd.FlexibleOpenGeographies
{
    public interface IContextFactory
    {
        IFogContext Create();
        NpgsqlConnection CreatePostGisOpenConnection();
    }
}
