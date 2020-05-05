using System;
using DAL.Base;
using Domain.Identity;

namespace Domain
{
    public class AppUsersScreen : DomainEntity
    {
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; } = default!;

        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = default!;
    }
}
