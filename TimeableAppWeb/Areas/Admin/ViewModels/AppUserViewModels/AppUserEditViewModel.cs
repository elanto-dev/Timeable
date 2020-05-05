using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeableAppWeb.Areas.Admin.ViewModels.AppUserViewModels
{
    public class AppUserEditViewModel
    {
        public Guid AppUserId { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; } = default!;

        [DisplayName("Full name")]
        public string FullName { get; set; } = default!;

        [DisplayName("Schedule management")]
        public bool ScheduleManagement { get; set; }

        [DisplayName("Events management")]
        public bool EventsManagement { get; set; }

        [DisplayName("Screen management")]
        public bool ScreenManagement { get; set; }
    }
}
