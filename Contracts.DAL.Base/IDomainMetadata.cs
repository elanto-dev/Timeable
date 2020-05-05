using System;

namespace Contracts.DAL.Base
{
    public interface IDomainMetadata
    {
        string? CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }

        string? ChangedBy { get; set; }
        DateTime ChangedAt { get; set; }
    }
}
