namespace BugNetCore.Models.Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            new BaseEntityWithAuditConfiguration<User>().Configure(builder);



            builder
                .HasIndex(u => u.Email)
                .IsUnique();

        }
    }
}
