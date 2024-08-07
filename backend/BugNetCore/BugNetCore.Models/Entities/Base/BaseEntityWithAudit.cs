namespace BugNetCore.Models.Entities.Base
{
    public class BaseEntityWithAudit : BaseEntity
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModified { get; set; }
    }
}
