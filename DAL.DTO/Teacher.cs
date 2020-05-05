using System;

namespace DAL.DTO
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string FullName { get; set; } = default!;
        public string? Role { get; set; }
    }
}
