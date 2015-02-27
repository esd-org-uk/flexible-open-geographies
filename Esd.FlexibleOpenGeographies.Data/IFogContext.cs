using System;
using System.Data.Entity;

namespace Esd.FlexibleOpenGeographies.Data
{
    public interface IFogContext : IDisposable
    {
        DbSet<AreaComposition> AreaCompositions { get; set; }
        DbSet<AreaDetail> AreaDetails { get; set; }
        DbSet<AreaAlternateLabel> AreaAlternateLabels { get; set; }
        DbSet<AreaTypeAlternateLabel> AreaTypeAlternateLabels { get; set; }
        DbSet<AreaResource> AreaResources { get; set; }
        DbSet<AreaTypeResource> AreaTypeResources { get; set; }
        DbSet<AreaType> AreaTypes { get; set; }
        DbSet<AreaTypeGroupMember> AreaTypeGroupMembers { get; set; }
        DbSet<Period> Periods { get; set; }
        DbSet<MetricType> MetricTypes { get; set; }
        DbSet<Metric> Metrics { get; set; }
        DbSet<MetricAggregation> MetricAggregations { get; set; }    
        DbSet<ReservedCode> ReservedCodes { get; set; }
        DbSet<TypeHierarchy> TypeHierarchies { get; set; }
        DbSet<Upload> Uploads { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Organisation> Organisations { get; set; }
        DbSet<MetricUploadPermissionLevel> MetricUploadPermissionLevels { get; set; }
        int SaveChanges();
        Database Database { get; }
    }
}
