using Esd.FlexibleOpenGeographies.Data;
using Npgsql;
using System.Configuration;

namespace Esd.FlexibleOpenGeographies
{
    public class ContextFactory : IContextFactory
    {
        public IFogContext Create()
        {
            return new FogContext();
        }

        public NpgsqlConnection CreatePostGisOpenConnection()
        {
            var connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgisConnection"].ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
