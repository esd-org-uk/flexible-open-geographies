namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class TypeHierarchyBasic
    {
        public string TypeCode { get; set; }
        public string ChildTypeCode { get; set; }
        public bool IsPrimary { get; set; }
        public bool CoversWhole { get; set; }
    }
}
