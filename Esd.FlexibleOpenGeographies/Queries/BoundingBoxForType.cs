using Esd.FlexibleOpenGeographies.Dtos;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class BoundingBoxForType : IQuerySingle<BoundingBox>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;

        public BoundingBoxForType(IContextFactory contextFactory, string typeCode)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
        }

        public BoundingBox Find()
        {
            using (var context = _contextFactory.Create())
            {
                var isGroup = context.AreaTypes.AsNoTracking()
                                     .Where(x => x.Code == _typeCode)
                                     .Select(x => x.IsGroup)
                                     .SingleOrDefault()
                                     .GetValueOrDefault(false);
                return !isGroup
                    ? BoundingBox(_typeCode)
                    : BoundingBox(context.AreaTypeGroupMembers.AsNoTracking()
                                         .Where(x => x.TypeCode == _typeCode)
                                         .Select(x => x.ChildTypeCode)
                                         .ToList());
            }
        }

        private BoundingBox BoundingBox(IList<string> typeCodes)
        {
            if (!typeCodes.Any()) return null;
            var boundingBoxes = typeCodes.Select(BoundingBox).Where(x => x != null).ToList();
            return boundingBoxes.Any()
                ? new BoundingBox
                {
                    MaximumX = boundingBoxes.Max(x => x.MaximumX),
                    MaximumY = boundingBoxes.Max(x => x.MaximumY),
                    MinimumX = boundingBoxes.Min(x => x.MinimumX),
                    MinimumY = boundingBoxes.Min(x => x.MinimumY)
                }
                : null;
        }

        private BoundingBox BoundingBox(string typeCode)
        {
            BoundingBox bounds = null;
            using (var connection = _contextFactory.CreatePostGisOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT ST_XMIN(extent) AS minx, ST_YMIN(extent) AS miny, ST_XMAX(extent) AS maxx, ST_YMAX(extent) AS maxy FROM (SELECT ST_EXTENT(shape) as extent from area where area_type_code = :typecode) a";
                    command.CommandType = CommandType.Text;
                    var typeCodeParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "typecode",
                        Value = typeCode
                    };
                    command.Parameters.Add(typeCodeParameter);
                    using (var reader = command.ExecuteReader())
                        if (reader.Read() && reader["minx"] is double)
                            bounds = new BoundingBox
                            {
                                MinimumX = (double)reader["minx"],
                                MaximumX = (double)reader["maxx"],
                                MinimumY = (double)reader["miny"],
                                MaximumY = (double)reader["maxy"]
                            };
                }
                connection.Close();
            }
            return bounds;
        }
    }
}
