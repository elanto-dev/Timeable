using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class Event
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(100, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Name), Prompt = nameof(Name), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public string Name { get; set; } = default!;

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(50, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Place), Prompt = nameof(Place), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public string Place { get; set; } = default!;

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(StartDateTime), Prompt = nameof(StartDateTime), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(EndDateTime), Prompt = nameof(EndDateTime), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public DateTime EndDateTime { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(ShowStartDateTime), Prompt = nameof(ShowStartDateTime), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public DateTime ShowStartDateTime { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(ShowEndDateTime), Prompt = nameof(ShowEndDateTime), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public DateTime ShowEndDateTime { get; set; }
    }
}
