namespace Light.Domain.Entities.Interfaces;

public interface IAuditTime
{
    DateTimeOffset Created { get; set; }

    DateTimeOffset? LastModified { get; set; }
}