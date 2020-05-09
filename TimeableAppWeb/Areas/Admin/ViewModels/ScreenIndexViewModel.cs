using System.Collections.Generic;
using BLL.DTO;

namespace TimeableAppWeb.Areas.Admin.ViewModels
{
    public class ScreenIndexViewModel
    {
        public Screen Screen { get; set; } = default!;
        public List<PictureInScreen>? Promotions { get; set; }
        public Picture? BackgroundPicture { get; set; }
        public bool ShowPrefixError { get; set; }
        public string PrefixError => Resources.Domain.ScreenView.Screen.PrefixError;
        public bool UserHasRightsToEdit { get; set; }
    }
}
