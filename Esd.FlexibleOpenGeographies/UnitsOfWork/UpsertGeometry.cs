using Npgsql;
using System;
using System.Data;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class UpsertGeometry : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _content;
        private readonly string _areaCode;
        private readonly string _typeCode;
        private readonly IGeoContentTypeDetector _geoContentTypeDetector;
        private readonly IFragmentExtractorFactory _fragmentExtractorFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UpsertGeometry(
            IContextFactory contextFactory, 
            string content, 
            string areaCode,
            string typeCode,
            IGeoContentTypeDetector geoContentTypeDetector,
            IFragmentExtractorFactory fragmentExtractorFactory,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _contextFactory = contextFactory;
            _content = content;
            _areaCode = areaCode;
            _typeCode = typeCode;
            _geoContentTypeDetector = geoContentTypeDetector;
            _fragmentExtractorFactory = fragmentExtractorFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Execute()
        {
            var type = _geoContentTypeDetector.DetectFromContent(_content);
            var fragment = _fragmentExtractorFactory.CreateForType(type).Extract(_content);
            if (fragment == null) return;
            try
            {
                using (var connection = _contextFactory.CreatePostGisOpenConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = CommandName(type);
                        command.CommandType = CommandType.StoredProcedure;
                        var contentParameter = new NpgsqlParameter { ParameterName = "content", DbType = DbType.String, Value = fragment };
                        var areaCodeParameter = new NpgsqlParameter { ParameterName = "areacode", DbType = DbType.String, Value = _areaCode };
                        var typeCodeParameter = new NpgsqlParameter { ParameterName = "typecode", DbType = DbType.String, Value = _typeCode };
                        command.Parameters.Add(contentParameter);
                        command.Parameters.Add(areaCodeParameter);
                        command.Parameters.Add(typeCodeParameter);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                _unitOfWorkFactory.CreateSetGeometryCalculationProcess(_typeCode, _areaCode, true).Execute();
            }
            catch
            {
                _unitOfWorkFactory.CreateSetGeometryCalculationProcess(_typeCode, _areaCode, false).Execute();
                throw;
            }
        }

        private static string CommandName(GeoContentType contentType)
        {
            switch (contentType)
            {
                case GeoContentType.GeoJson:
                    return "upsertAreaFromGeoJson";
                case GeoContentType.Kml:
                    return "upsertAreaFromKml";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
