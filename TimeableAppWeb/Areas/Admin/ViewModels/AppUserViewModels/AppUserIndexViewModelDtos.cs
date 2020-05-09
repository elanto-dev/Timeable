using System;
using System.ComponentModel.DataAnnotations;

namespace TimeableAppWeb.Areas.Admin.ViewModels.AppUserViewModels
{
    public class AppUserIndexViewModelDtos
    {
        public Guid Id { get; set; } = default!;

        [Display(Name = nameof(FullName), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public string FullName { get; set; } = default!;

        [Display(Name = nameof(Email), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public string Email { get; set; } = default!;

        [Display(Name = nameof(ScheduleManagement), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool ScheduleManagement { get; set; }

        [Display(Name = nameof(EventsManagement), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool EventsManagement { get; set; }

        [Display(Name = nameof(ScreenManagement), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool ScreenManagement { get; set; }

        [Display(Name = nameof(UserIsHeadAdmin), ResourceType = typeof(Resources.Domain.AppUsersView.AppUser))]
        public bool UserIsHeadAdmin { get; set; }
        public bool UserHasScreen { get; set; }
    }
}
