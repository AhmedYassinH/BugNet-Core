
namespace BugNetCore.Models.Entities.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            new BaseEntityWithAuditConfiguration<Comment>().Configure(builder);
        }
    }
}
