using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Teacher : DomainEntity
    {
        [MaxLength(200)]
        public string FullName { get; set; } = default!;

        [MaxLength(100)]
        public string? Role { get; set; } 

        public ICollection<TeacherInSubjectEvent>? TeacherInSubjectEvents { get; set; }
    }
}
