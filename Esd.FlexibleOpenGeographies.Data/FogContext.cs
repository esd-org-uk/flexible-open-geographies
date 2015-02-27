using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;

namespace Esd.FlexibleOpenGeographies.Data
{
    public class FogContext : DbContext, IFogContext
    {
        public FogContext() : base("name=FogConnection") {}

        public DbSet<AreaComposition> AreaCompositions { get; set; }
        public DbSet<AreaDetail> AreaDetails { get; set; }
        public DbSet<AreaAlternateLabel> AreaAlternateLabels { get; set; }
        public DbSet<AreaTypeAlternateLabel> AreaTypeAlternateLabels { get; set; }
        public DbSet<AreaResource> AreaResources { get; set; }
        public DbSet<AreaTypeResource> AreaTypeResources { get; set; }
        public DbSet<AreaType> AreaTypes { get; set; }
        public DbSet<AreaTypeGroupMember> AreaTypeGroupMembers { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<MetricType> MetricTypes { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<MetricAggregation> MetricAggregations { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<MetricUploadPermissionLevel> MetricUploadPermissionLevels { get; set; }
        public DbSet<ReservedCode> ReservedCodes { get; set; }
        public DbSet<TypeHierarchy> TypeHierarchies { get; set; }

        public override int SaveChanges()
        {
            try { return base.SaveChanges(); }
            catch (DbEntityValidationException e)
            {
                var errorMessage = new StringBuilder();

                foreach (var error in e.EntityValidationErrors)
                {
                    errorMessage.AppendFormat("{0} failed validation \n", error.Entry.Entity.GetType());
                    foreach (var validationError in error.ValidationErrors)
                    {
                        errorMessage.AppendFormat("- {0} : {1}", validationError.PropertyName, validationError.ErrorMessage);
                        errorMessage.AppendLine();
                    }
                }

                throw new DbEntityValidationException(string.Format("Entity Validation Failed - errors follow:\n{0}", errorMessage), e);
            }
        }
    }
}
