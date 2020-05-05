using System;

namespace DAL.DTO
{
    public class Subject
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string SubjectCode { get; set; } = default!;
        public string SubjectName { get; set; } = default!;
    }
}
