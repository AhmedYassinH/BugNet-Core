


namespace BugNetCore.Models.Entities.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("xmin", TypeName = "xid")]
        [ConcurrencyCheck]
        public uint RowVersion { get; set; }
    }
}