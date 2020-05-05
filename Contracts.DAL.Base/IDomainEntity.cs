using System;

namespace Contracts.DAL.Base
{
    public interface IDomainEntity : IDomainEntity<int>
    {
    }

    public interface IDomainEntity<TKey> : IDomainMetadata
        where TKey : struct, IComparable
    {
        TKey Id { get; set; }
    }
}
