namespace BugNetCore.Models.Entities
{

    [Table("SeriLogs", Schema = "Logging")]
    [EntityTypeConfiguration(typeof(SeriLogEntryConfiguration))]
    public class SeriLogEntry
    {
        public string? Message { get; set; }

        public string? MessageTemplate { get; set; }

        public string? Level { get; set; }

        public DateTime? RaiseDate { get; set; }

        public string? Exception { get; set; }

        public string? Properties { get; set; }

        public string? PropsTest { get; set; }

        public string? MachineName { get; set; }

        public string? FilePath { get; set; }

        public string? ApplicationName { get; set; }

        public string? MemberName { get; set; }

        public int? LineNumber { get; set; }
    }
}
