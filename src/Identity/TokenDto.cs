namespace Light.Identity;

public record TokenDto(string AccessToken, double ExpiresIn, string? RefreshToken);