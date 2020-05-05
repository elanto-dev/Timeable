using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class Event
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        [MaxLength(50)]
        public string Place { get; set; } = default!;
        [DisplayName("Starts")]
        public DateTime StartDateTime { get; set; }
        [DisplayName("Ends")]
        public DateTime EndDateTime { get; set; }
        [DisplayName("Start showing")]
        public DateTime ShowStartDateTime { get; set; }
        [DisplayName("End showing")]
        public DateTime ShowEndDateTime { get; set; }
    }
}
