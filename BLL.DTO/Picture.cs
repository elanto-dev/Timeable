using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class Picture
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        [MaxLength(255)]
        public string Path { get; set; } = default!;

        [MaxLength(200)]
        public string? Comment { get; set; } = default!;
    }
}
