using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class Screen
    {
        public int Id { get; set; }
        [MaxLength(36)]
        public string UniqueIdentifier { get; set; } = default!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        [MaxLength(20)]
        public string Prefix { get; set; } = default!;

        [DisplayName("Screen is active")]
        public bool IsActive { get; set; }

        [DisplayName("Show schedule")]
        public string ShowScheduleSeconds { get; set; } = default!;
    }
}
