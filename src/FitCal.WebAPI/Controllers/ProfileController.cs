using System.Security.Claims;
using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitCal.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    private Guid GetAuthUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(idStr, out var authUserId))
            throw new UnauthorizedAccessException("Invalid user id");
        return authUserId;
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserProfileResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileResponseDTO>> GetProfile()
    {
        var authUserId = GetAuthUserId();
        var result = await _profileService.GetProfileAsync(authUserId);
        return Ok(result);
    }

    [HttpPost("save")]
    [ProducesResponseType(typeof(UserProfileResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserProfileResponseDTO>> SaveProfile(
        [FromBody] UserProfileRequestDTO request)
    {
        var authUserId = GetAuthUserId();
        Console.WriteLine($"SAVE PROFILE authUserId={authUserId}");
        var result = await _profileService.SaveProfileAsync(authUserId, request);
        return Ok(result);
    }
}