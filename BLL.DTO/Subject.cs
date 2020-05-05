using System;
using System.ComponentModel;
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

        [MaxLength(30, ErrorMessage = "Subject code must be maximum 30 symbols long.")]
        [DisplayName("Subject code")]
        public string SubjectCode { get; set; } = default!;

        [MaxLength(200)]
        [DisplayName("Subject name")]
        public string SubjectName { get; set; } = default!;
    }
}
