using System;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public abstract class DomainEntity : DomainEntity<int>, IDomainEntity
    {
    }

    public abstract class DomainEntity<TKey> : IDomainEntity<TKey>
        where TKey : struct, IComparable
    {
        public virtual TKey Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;
    }
}
