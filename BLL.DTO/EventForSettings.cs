namespace BLL.DTO
{
    public class EventForSettings
    {
        public int Id { get; set; }
        public string StartDateTimeString { get; set; } = default!;
        public string EndDateTimeString { get; set; } = default!;
        public string EventName { get; set; } = default!;
        public string Place { get; set; } = default!;
        public string ShowFromDateTimeString { get; set; } = default!;
        public string ShowToDateTimeString { get; set; } = default!;
    }
}
