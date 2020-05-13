using System.ComponentModel.DataAnnotations;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimeableAppWeb.Areas.Admin.ViewModels
{
    public class PictureCreateViewModel
    {
        public Picture Picture { get; set; } = default!;
        public int ScreenId { get; set; } = default!;
        public bool IsBackgroundPicture { get; set; }
        public SelectList? PromotionSecondsSelectList { get; set; } 
        public string? ShowPromotionSecondsString { get; set; } 
        public SelectList? ScheduleSecondsSelectList { get; set; }
        public string? ShowScheduleSecondsString { get; set; }

        [Display(Name = nameof(PictureFromUrl), Prompt = nameof(PictureFromUrl), ResourceType = typeof(Resources.Domain.PictureView.Picture))]
        public bool PictureFromUrl { get; set; }
    }
}
