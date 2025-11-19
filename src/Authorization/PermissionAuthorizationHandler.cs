using Microsoft.AspNetCore.Authorization;

namespace Light.Authorization;

public abstract class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>;