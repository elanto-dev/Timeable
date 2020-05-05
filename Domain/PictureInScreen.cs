using DAL.Base;

namespace Domain
{
    public class PictureInScreen : DomainEntity
    {
        public int PictureId { get; set; }
        public Picture Picture { get; set; } = default!;

        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = default!;

        public bool IsBackgroundPicture { get; set; }

        public int? ShowAddSeconds { get; set; }
    }
}
