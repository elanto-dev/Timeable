using System.Collections.Generic;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimeableAppWeb.Areas.Admin.ViewModels
{
    public class ScreenCreateEditViewModel
    {
        public Screen Screen { get; set; } = default!;
        public List<PictureInScreen>? PictureInScreens { get; set; }
        public bool ScheduleAlwaysShown { get; set; }
        public Dictionary<int, SelectList> PromotionSecondsSelectListDictionary { get; set; } = default!;
        public Dictionary<int, string> ShowPromotionSecondsStringDictionary { get; set; } = default!;
        public SelectList ScheduleSecondsSelectList { get; set; } = default!;
        public string ShowScheduleSecondsString { get; set; } = default!;
        public string? ScreenOldPrefix { get; set; }
    }
}
