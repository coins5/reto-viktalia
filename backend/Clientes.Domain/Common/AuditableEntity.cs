namespace Clientes.Domain.Common;

public abstract class AuditableEntity
{
    public long Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; }

    public void MarkCreated(DateTimeOffset timestamp)
    {
        var utc = timestamp.UtcDateTime;
        CreatedAt = utc;
        UpdatedAt = utc;
        IsDeleted = false;
    }

    public void MarkUpdated(DateTimeOffset timestamp)
    {
        UpdatedAt = timestamp.UtcDateTime;
    }

    public void MarkDeleted(DateTimeOffset timestamp)
    {
        IsDeleted = true;
        UpdatedAt = timestamp.UtcDateTime;
    }
}
