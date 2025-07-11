namespace Light.Domain.Entities.Interfaces;

public interface IModified
{
    DateTimeOffset? LastModified { get; set; }

    string? LastModifiedBy { get; set; }
}