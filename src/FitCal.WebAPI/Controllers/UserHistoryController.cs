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
public class UserHistoryController : ControllerBase
{
    private readonly IUserHistoryService _userHistoryService;

    public UserHistoryController(IUserHistoryService userHistoryService)
    {
        _userHistoryService = userHistoryService;
    }

    private Guid GetAuthUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(idStr, out var authUserId))
            throw new UnauthorizedAccessException("Invalid user id");

        return authUserId;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserHistoryResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserHistoryResponseDTO>> CreateUserHistoryRecord([FromBody] UserHistoryRequestDTO request)
    {
        var authUserId = GetAuthUserId();
        var result = await _userHistoryService.AddUserHistoryRecordAsync(authUserId, request);
        return CreatedAtAction(nameof(GetUserHistoryRecordById), new { id = result.UserHistoryId }, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserHistoryResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserHistoryResponseDTO>> GetUserHistoryRecordById(int id)
    {
        var result = await _userHistoryService.GetUserHistoryRecordByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(IReadOnlyList<UserHistoryResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserHistoryResponseDTO>>> GetMyUserHistory()
    {
        var authUserId = GetAuthUserId();
        var result = await _userHistoryService.GetUserHistoryAsync(authUserId);
        return Ok(result);
    }

    [HttpGet("me/date/{date}")]
    [ProducesResponseType(typeof(IReadOnlyList<UserHistoryResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<UserHistoryResponseDTO>>> GetMyUserHistoryByDate(string date)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
            throw new ArgumentException("Неверный формат даты. Используйте yyyy-MM-dd");

        var authUserId = GetAuthUserId();
        var result = await _userHistoryService.GetUserHistoryByDateAsync(authUserId, parsedDate);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserHistoryRecord(int id)
    {
        var authUserId = GetAuthUserId();
        await _userHistoryService.RemoveUserHistoryRecordAsync(authUserId, id);
        return NoContent();
    }
}
