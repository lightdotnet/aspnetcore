using Microsoft.AspNetCore.Authorization;

namespace Light.Authorization;

public record PermissionRequirement(string Permission) : IAuthorizationRequirement;