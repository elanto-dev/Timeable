using System.ComponentModel.DataAnnotations;

namespace TimeableAppWeb.Areas.Admin.ViewModels.AppUserViewModels
{
    public class AppUserCreateViewModel
    {
        [EmailAddress]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Email), Prompt = nameof(Email), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public string Email { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [StringLength(100, ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", ErrorMessageResourceType = typeof(Resources.Views.Common), MinimumLength = 6)]
        [Display(Name = nameof(Password), Prompt = nameof(Password), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(ConfirmPassword), Prompt = nameof(ConfirmPassword), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        [Compare("Password", ErrorMessageResourceName = "ErrorMessage_PasswordDontMatch", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        public string ConfirmPassword { get; set; } = default!;

        [MinLength(1, ErrorMessageResourceName = "ErrorMessage_MinLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(100, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(FirstName), Prompt = nameof(FirstName), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public string FirstName { get; set; } = default!;

        [MinLength(1, ErrorMessageResourceName = "ErrorMessage_MinLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(100, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(LastName), Prompt = nameof(LastName), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public string LastName { get; set; } = default!;

        [Display(Name = nameof(ScheduleManagement), Prompt = nameof(ScheduleManagement), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool ScheduleManagement { get; set; }

        [Display(Name = nameof(EventsManagement), Prompt = nameof(EventsManagement), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool EventsManagement { get; set; }

        [Display(Name = nameof(ScreenManagement), Prompt = nameof(ScreenManagement), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool ScreenManagement { get; set; }
    }
}
