using System.Collections.Generic;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimeableAppWeb.Areas.Admin.ViewModels
{
    public class PictureCreateEditViewModel
    {
        public Picture Picture { get; set; } = default!;
        public int ScreenId { get; set; } = default!;
        public bool IsBackgroundPicture { get; set; }
        public SelectList PromotionSecondsSelectList { get; set; } = default!;
        public string? ShowPromotionSecondsString { get; set; } 
        public SelectList ScheduleSecondsSelectList { get; set; } = default!;
        public string? ShowScheduleSecondsString { get; set; }
    }
}
