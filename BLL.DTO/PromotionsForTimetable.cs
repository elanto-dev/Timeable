namespace BLL.DTO
{
    public class PromotionsForTimetable
    {
        public int PictureId { get; set; }
        public Picture Picture { get; set; } = default!;
        public int? ShowAddSeconds { get; set; }
    }
}
