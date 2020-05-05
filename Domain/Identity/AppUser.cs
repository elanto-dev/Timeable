using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.DAL.Base;
using DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppUser : IdentityUser<Guid>, IDomainEntity<Guid>
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        [MinLength(1), MaxLength(100)]
        public string FirstName { get; set; } = default!;

        [MinLength(1), MaxLength(100)]
        public string LastName { get; set; } = default!;

        public bool Activated { get; set; }

        public ICollection<AppUsersScreen>? AppUsersScreens { get; set; }
    }
}
