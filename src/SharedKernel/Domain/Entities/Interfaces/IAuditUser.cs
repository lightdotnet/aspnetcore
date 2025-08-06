namespace Light.Domain.Entities.Interfaces;

public interface IAuditUser
{
    string? CreatedBy { get; set; }

    string? LastModifiedBy { get; set; }
}
