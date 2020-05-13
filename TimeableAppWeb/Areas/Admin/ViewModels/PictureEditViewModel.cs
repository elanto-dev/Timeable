using System.ComponentModel.DataAnnotations;
using BLL.DTO;

namespace TimeableAppWeb.Areas.Admin.ViewModels
{
    public class PictureEditViewModel
    {
        public Picture Picture { get; set; } = default!;

        [Display(Name = nameof(PictureFromUrl), Prompt = nameof(PictureFromUrl), ResourceType = typeof(Resources.Domain.PictureView.Picture))]
        public bool PictureFromUrl { get; set; }
        public string OldPicturePath { get; set; } = default!;
        public string OldFileName { get; set; } = default!;
    }
}
