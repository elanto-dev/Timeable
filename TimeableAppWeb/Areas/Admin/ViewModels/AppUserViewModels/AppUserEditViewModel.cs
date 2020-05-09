using System;
using System.ComponentModel.DataAnnotations;

namespace TimeableAppWeb.Areas.Admin.ViewModels.AppUserViewModels
{
    public class AppUserEditViewModel
    {
        public Guid AppUserId { get; set; }

        [EmailAddress]
        [Display(Name = nameof(Email), Prompt = nameof(Email), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public string Email { get; set; } = default!;

        [Display(Name = nameof(FullName), Prompt = nameof(FullName), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public string FullName { get; set; } = default!;

        [Display(Name = nameof(ScheduleManagement), Prompt = nameof(ScheduleManagement), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool ScheduleManagement { get; set; }

        [Display(Name = nameof(EventsManagement), Prompt = nameof(EventsManagement), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool EventsManagement { get; set; }

        [Display(Name = nameof(ScreenManagement), Prompt = nameof(ScreenManagement), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool ScreenManagement { get; set; }
    }
}
