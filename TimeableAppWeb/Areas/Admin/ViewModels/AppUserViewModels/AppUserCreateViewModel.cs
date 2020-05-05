using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TimeableAppWeb.Areas.Admin.ViewModels.AppUserViewModels
{
    public class AppUserCreateViewModel
    {
        [Required]
        [EmailAddress]
        [DisplayName( "Email")]
        public string Email { get; set; } = default!;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = default!;

        [MinLength(1), MaxLength(100)]
        [DisplayName("First name")]
        public string FirstName { get; set; } = default!;

        [MinLength(1), MaxLength(100)]
        [DisplayName("Last name")]
        public string LastName { get; set; } = default!;

        [DisplayName("Schedule management")]
        public bool ScheduleManagement { get; set; }

        [DisplayName("Events management")]
        public bool EventsManagement { get; set; }

        [DisplayName("Screen management")]
        public bool ScreenManagement { get; set; }
    }
}
