namespace Light.Domain.Entities.Interfaces;

public interface IModificationTime
{
    DateTimeOffset? LastModified { get; set; }
}

public interface IModification : IModificationTime
{
    string? LastModifiedBy { get; set; }
}