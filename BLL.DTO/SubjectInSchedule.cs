using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class SubjectInSchedule
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = default!;
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = default!;

        public string UniqueIdentifier { get; set; } = default!;

        [Display(Name = nameof(SubjectType), Prompt = nameof(SubjectType), ResourceType = typeof(Resources.Domain.SubjectInSchedule.SubjectInSchedule))]
        public int SubjectType { get; set; } = default!;

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(150, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Rooms), Prompt = nameof(Rooms), ResourceType = typeof(Resources.Domain.SubjectInSchedule.SubjectInSchedule))]
        public string Rooms { get; set; } = default!;

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(500, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Groups), Prompt = nameof(Groups), ResourceType = typeof(Resources.Domain.SubjectInSchedule.SubjectInSchedule))]
        public string Groups { get; set; } = default!;

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(StartDateTime), Prompt = nameof(StartDateTime), ResourceType = typeof(Resources.Domain.SubjectInSchedule.SubjectInSchedule))]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(EndDateTime), Prompt = nameof(EndDateTime), ResourceType = typeof(Resources.Domain.SubjectInSchedule.SubjectInSchedule))]
        public DateTime EndDateTime { get; set; }
    }
}
