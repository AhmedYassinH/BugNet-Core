namespace BugNetCore.Models.DTOs.Comment
{
    public class CreateCommentResponseDto : BaseEntityWithAudit
    {
        public ReadUserResponseDto Sender { get; set; }

        public ReadBugResponseDto Bug { get; set; }

        public string CommentText { get; set; }

    }
}
