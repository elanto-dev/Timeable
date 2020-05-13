using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class Picture
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        [MaxLength(255, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Path), Prompt = nameof(Path), ResourceType = typeof(Resources.Domain.PictureView.Picture))]
        public string Path { get; set; } = default!;

        [MaxLength(200, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Comment), Prompt = nameof(Comment), ResourceType = typeof(Resources.Domain.PictureView.Picture))]
        public string? Comment { get; set; } = default!;
    }
}
