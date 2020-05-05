using System;

namespace DAL.DTO
{
    public class Picture
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string Path { get; set; } = default!;
        public string? Comment { get; set; } = default!;
    }
}
