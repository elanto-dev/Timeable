using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class Subject
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(30, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(SubjectCode), Prompt = nameof(SubjectCode), ResourceType = typeof(Resources.Domain.SubjectInSchedule.SubjectInSchedule))]
        public string SubjectCode { get; set; } = default!;

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(200, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(SubjectName), Prompt = nameof(SubjectName), ResourceType = typeof(Resources.Domain.SubjectInSchedule.SubjectInSchedule))]
        public string SubjectName { get; set; } = default!;
    }
}
