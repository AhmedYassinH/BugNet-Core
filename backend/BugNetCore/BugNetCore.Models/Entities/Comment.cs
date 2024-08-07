namespace BugNetCore.Models.Entities
{
    public class Comment  : BaseEntityWithAudit
    {
        [Required]
        public Guid SenderId { get; set; } // FK

        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }


        [Required]
        public Guid BugId { get; set; } // FK

        [ForeignKey(nameof(BugId))]
        public Bug Bug { get; set; }

        [Required]
        public string CommentText { get; set; }

    }
}
