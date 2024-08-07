namespace BugNetCore.Models.Entities.Base
{
    [EntityTypeConfiguration(typeof(BaseEntityWithAuditConfiguration))]
    public class BaseEntityWithAudit : BaseEntity
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModified { get; set; }
    }
}
