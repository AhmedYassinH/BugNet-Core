namespace BugNetCore.Models.DTOs.Comment
{
    public class ReadCommentResponseMinimalDto : BaseEntityWithAudit
    {
        public string SenderName { get; set; }

        public Guid BugId { get; set; }

        public string CommentText { get; set; }

    }
}
