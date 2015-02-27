using Npgsql;
using System.Collections.Generic;
using System.Data;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class UpsertCalculatedGeometry : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _areaCode;
        private readonly string _typeCode;
        private readonly IEnumerable<string> _childAreaCodes;
        private readonly string _childTypeCode;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UpsertCalculatedGeometry(IContextFactory contextFactory, string areaCode, string typeCode, IEnumerable<string> childAreaCodes, string childTypeCode, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _contextFactory = contextFactory;
            _areaCode = areaCode;
            _typeCode = typeCode;
            _childAreaCodes = childAreaCodes;
            _childTypeCode = childTypeCode;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Execute()
        {
            try
            {
                using (var connection = _contextFactory.CreatePostGisOpenConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "upsertareafromchildareas";
                        command.CommandType = CommandType.StoredProcedure;
                        var childAreasParameter = new NpgsqlParameter { ParameterName = "childareas", DbType = DbType.String, Value = string.Join(",", _childAreaCodes) };
                        var childTypeCodeParameter = new NpgsqlParameter { ParameterName = "childtypecode", DbType = DbType.String, Value = _childTypeCode };
                        var areaCodeParameter = new NpgsqlParameter { ParameterName = "areacode", DbType = DbType.String, Value = _areaCode };
                        var typeCodeParameter = new NpgsqlParameter { ParameterName = "typecode", DbType = DbType.String, Value = _typeCode };
                        command.Parameters.Add(childAreasParameter);
                        command.Parameters.Add(childTypeCodeParameter);
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
    }
}
