using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class Screen
    {
        public int Id { get; set; }
        [MaxLength(36)]
        public string UniqueIdentifier { get; set; } = default!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(20, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Prefix), Prompt = nameof(Prefix), ResourceType = typeof(Resources.Domain.ScreenView.Screen))]
        public string Prefix { get; set; } = default!;

        [Display(Name = nameof(IsActive), Prompt = nameof(IsActive), ResourceType = typeof(Resources.Domain.ScreenView.Screen))]
        public bool IsActive { get; set; }

        [Display(Name = nameof(ShowScheduleSeconds), Prompt = nameof(ShowScheduleSeconds), ResourceType = typeof(Resources.Domain.ScreenView.Screen))]
        public string ShowScheduleSeconds { get; set; } = default!;
    }
}
