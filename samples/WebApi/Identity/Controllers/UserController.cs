using Light.Identity;
using Light.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Identity.Controllers;

public class UserController(
    IUserService userService,
    UserManager<User> userManager) : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var res = await userService.GetAllAsync(cancellationToken);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        var res = await userService.GetByIdAsync(id);
        return Ok(res);
    }

    [HttpGet("by_user_name/{userName}")]
    public async Task<IActionResult> GetByUserNameAsync([FromRoute] string userName)
    {
        var res = await userService.GetByUserNameAsync(userName);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateUserRequest request)
    {
        var res = await userService.CreateAsync(request);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync([FromBody] UserDto request)
    {
        var res = await userService.UpdateAsync(request);
        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        return Ok(await userService.DeleteAsync(id));
    }

    [HttpPut("force_password")]
    public async Task<IActionResult> ForcePasswordAsync(string userId, string password)
    {
        var res = await userService.ForcePasswordAsync(userId, password);
        return Ok(res);
    }

    [HttpGet("by_claim/{key}/{value}")]
    public async Task<IActionResult> GetByClaimAsync(string key, string value)
    {
        var res = await userService.GetUsersHasClaim(key, value);
        return Ok(res);
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var user = await userManager.FindByNameAsync("super");

        await userManager.AddLoginAsync(user!, new UserLoginInfo(
            "AD",
            "domain.local",
            "DomainUserLogin"));

        var logins = userManager.GetLoginsAsync(user!);

        return Ok(logins);
    }
}