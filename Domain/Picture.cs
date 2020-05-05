using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Picture : DomainEntity
    {
        [MaxLength(255)]
        public string Path { get; set; } = default!;

        [MaxLength(200)]
        public string? Comment { get; set; } 

        public ICollection<PictureInScreen>? PictureInScreens { get; set; }
    }
}
