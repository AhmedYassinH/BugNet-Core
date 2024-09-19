namespace BugNetCore.Models.DTOs.Base
{
    public interface IImageUploadable
    {
        IFormFile? Image { get; set; }
    }
}
