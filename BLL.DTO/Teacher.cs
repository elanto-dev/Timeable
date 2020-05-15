using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(200, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(TeacherName), Prompt = nameof(TeacherName), ResourceType = typeof(Resources.Domain.SubjectInSchedule.SubjectInSchedule))]
        public string TeacherName { get; set; } = default!;

        [MaxLength(100, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(TeacherRole), Prompt = nameof(TeacherRole), ResourceType = typeof(Resources.Domain.SubjectInSchedule.SubjectInSchedule))]
        public string? TeacherRole { get; set; }
    }
}
