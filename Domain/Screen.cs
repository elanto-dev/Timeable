using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Screen : DomainEntity
    {
        [MaxLength(36)]
        public string UniqueIdentifier { get; set; } = default!;

        [MaxLength(20)]
        public string Prefix { get; set; } = default!;

        public bool IsActive { get; set; }

        public int? ShowScheduleSeconds { get; set; }

        public ICollection<PictureInScreen>? PictureInScreens { get; set; }

        public ICollection<AppUsersScreen>? AppUserScreens { get; set; }
    }
}
