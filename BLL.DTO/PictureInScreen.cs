using System;
using System.ComponentModel;

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
        [DisplayName("This picture is schedule background")]
        public bool IsBackgroundPicture { get; set; }
        [DisplayName("Show promotion")]
        public string ShowAddSeconds { get; set; } = default!;
    }
}
