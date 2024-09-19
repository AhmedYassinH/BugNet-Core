namespace BugNetCore.Models.Entities.Configuration
{
    public class SeriLogEntryConfiguration : IEntityTypeConfiguration<SeriLogEntry>
    {
        public void Configure(EntityTypeBuilder<SeriLogEntry> builder)
        {


            builder.HasNoKey();

            builder.Property(e => e.Properties).HasColumnType("jsonb");
            builder.Property(e => e.PropsTest).HasColumnType("jsonb");

        }
    }
}
