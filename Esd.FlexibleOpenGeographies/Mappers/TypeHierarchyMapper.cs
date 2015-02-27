using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public static class TypeHierarchyMapper
    {
        public static TypeHierarchyBasic MapBasic(TypeHierarchy typeHierarchy)
        {
            return new TypeHierarchyBasic
            {
                TypeCode = typeHierarchy.TypeCode,
                ChildTypeCode = typeHierarchy.ChildTypeCode
            };
        }
    }
}
