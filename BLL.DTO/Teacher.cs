using System;
using System.ComponentModel;
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

        [MaxLength(200)]
        [DisplayName("Full name")]
        public string FullName { get; set; } = default!;

        [MaxLength(100)]
        [DisplayName("Role")]
        public string? Role { get; set; }
    }
}
