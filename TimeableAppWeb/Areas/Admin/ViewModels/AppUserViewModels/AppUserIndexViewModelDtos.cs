using System;
using System.ComponentModel;

namespace TimeableAppWeb.Areas.Admin.ViewModels.AppUserViewModels
{
    public class AppUserIndexViewModelDtos
    {
        public Guid Id { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool ScheduleManagement { get; set; }
        public bool EventsManagement { get; set; }
        public bool ScreenManagement { get; set; }
        public bool UserHasScreen { get; set; }
    }
}
