namespace BugNetCore.Models.Entities.Configuration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            new BaseEntityWithAuditConfiguration<Project>().Configure(builder);
        }
    }
    
}
