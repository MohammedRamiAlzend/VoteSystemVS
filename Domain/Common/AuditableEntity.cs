﻿// Ignore Spelling: Auditable

namespace Domain.Common;

public class AuditableEntity : Entity
{
    protected AuditableEntity()
    {

    }
    protected AuditableEntity(int id) : base(id)
    {

    }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset LastModifiedUtc { get; set; }
    public string? LastModifiedBy { get; set; }


}
