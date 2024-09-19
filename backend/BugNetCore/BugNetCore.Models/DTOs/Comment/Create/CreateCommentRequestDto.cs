namespace BugNetCore.Models.DTOs.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        public Guid SenderId { get; set; } // FK

        [Required]
        public Guid BugId { get; set; } // FK

        [Required]
        public string CommentText { get; set; }

    }

}
