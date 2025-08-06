namespace Light.Domain.Entities.Interfaces;

public interface ICreationTime
{
    DateTimeOffset Created { get; set; }
}

public interface ICreation : ICreationTime
{
    string? CreatedBy { get; set; }
}