namespace Light.Domain.Entities.Interfaces;

public interface ICreated
{
    DateTimeOffset Created { get; set; }

    string? CreatedBy { get; set; }
}