using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class PictureInScreen
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public int PictureId { get; set; }
        public Picture Picture { get; set; } = default!;
        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = default!;
        public bool IsBackgroundPicture { get; set; }

        [Display(Name = nameof(ShowAddSeconds), Prompt = nameof(ShowAddSeconds), ResourceType = typeof(Resources.Domain.PictureView.Picture))]
        public string ShowAddSeconds { get; set; } = default!;
    }
}
