using System;
using System.Collections.Generic;

namespace HTMLParser.DTO
{
    public class TimePlanEvent
    {
        public string EventIdentifier => $"{SubjectCode}-{StartDateTime:yyyyMMddHHmmss}-{EndDateTime:yyyyMMddHHmmss}";
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string SubjectCode { get; set; } = default!;
        public string SubjectName { get; set; } = default!;
        public ICollection<string> LecturersWithRoles { get; set; } = new List<string>();
        public string SubjectEventType { get; set; } = default!;
        public ICollection<string> Groups { get; set; } = new List<string>();
        public ICollection<string> Locations { get; set; } = new List<string>();
        public string Comment { get; set; } = default!;

        public override string ToString() =>
            $"[{EventIdentifier}] {StartDateTime} - {EndDateTime}: [{SubjectCode}] {SubjectName} ({SubjectEventType}) " +
            $"(Lecturers: {string.Join(", ", LecturersWithRoles ?? new List<string>())}) " +
            $"Rooms: {string.Join(", ", Locations ?? new List<string>())} " +
            $"for Groups: {string.Join(", ", Groups ?? new List<string>())}, Comment: '{Comment}'.";
    }
}
