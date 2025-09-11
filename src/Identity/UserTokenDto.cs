namespace Light.Identity;

public class UserTokenDto
{
    public string Id { get; set; } = null!;

    public DateTimeOffset? ExpiresAt { get; set; }

    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }

    public string? DeviceId { get; set; }

    public string? DeviceName { get; set; }

    public string? IpAddress { get; set; }

    public string? MacAddress { get; set; }
}