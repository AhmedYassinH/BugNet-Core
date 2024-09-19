namespace BugNetCore.Models.DTOs.Base
{
    public class BaseDto
    {
        public Guid Id { get; set; }

        public uint RowVersion { get; set; }
    }
}