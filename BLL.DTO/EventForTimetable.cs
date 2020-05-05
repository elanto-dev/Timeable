namespace BLL.DTO
{
    public class EventForTimetable
    { 
        public int Id { get; set; }
        public string HappeningDateTime { get; set; } = default!;
        public string EventName { get; set; } = default!;
        public string Place { get; set; } = default!;
    }
}
