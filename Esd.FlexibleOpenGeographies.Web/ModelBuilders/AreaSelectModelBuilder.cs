using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaSelectModelBuilder
    {
        public static AreaSelectModel EmptyModel { get { return new AreaSelectModel(); } }

        public static AreaSelectModel WithTypes(this AreaSelectModel model, IEnumerable<AreaTypeBasic> types)
        {
            model.AreaTypes = types;
            return model;
        }

        public static AreaSelectModel WithAreas(this AreaSelectModel model, IEnumerable<AreaBasic> areas)
        {
            model.Areas = areas;
            return model;
        }

        public static AreaSelectModel WithParameters(
            this AreaSelectModel model, 
            string typeCode, 
            string areaCode, 
            int? areaId, 
            IQueryFactory queryFactory)
        {
            if (areaId.HasValue)
            {
                var area = queryFactory.CreateAreaBasicWithTypeForIdQuery(areaId.Value).Find();
                if (area != null)
                {
                    model.AreaId = area.Id;
                    model.TypeCode = area.TypeCode;
                }
            }
            else if (!string.IsNullOrEmpty(typeCode))
            {
                model.TypeCode = typeCode;
                if (!string.IsNullOrEmpty(areaCode))
                    model.AreaId = queryFactory.CreateAreaIdForTypeAndCodeQuery(typeCode, areaCode).Find();
            }
            return model;
        }
    }
}