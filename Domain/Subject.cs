using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Subject : DomainEntity
    {
        [MaxLength(30)]
        public string SubjectCode { get; set; } = default!;

        [MaxLength(200)]
        public string SubjectName { get; set; } = default!;

        public ICollection<SubjectInSchedule>? SubjectInSchedules { get; set; }
    }
}
