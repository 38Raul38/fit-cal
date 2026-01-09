using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitCal.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserHistoryController : ControllerBase
{
    private readonly IUserHistoryService _userHistoryService;

    public UserHistoryController(IUserHistoryService userHistoryService)
    {
        _userHistoryService = userHistoryService;
    }

    // POST: api/userhistory
    [HttpPost]
    [ProducesResponseType(typeof(UserHistoryResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserHistoryResponseDTO>> CreateUserHistoryRecord([FromBody] UserHistoryRequestDTO request)
    {
        var result = await _userHistoryService.AddUserHistoryRecordAsync(request);
        return CreatedAtAction(nameof(GetUserHistoryRecordById), new { id = result.UserHistoryId }, result);
    }

    // GET: api/userhistory/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserHistoryResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserHistoryResponseDTO>> GetUserHistoryRecordById(int id)
    {
        var result = await _userHistoryService.GetUserHistoryRecordByIdAsync(id);
        return Ok(result);
    }

    // GET:  api/userhistory/user/{userInformationId}
    [HttpGet("user/{userInformationId}")]
    [ProducesResponseType(typeof(IReadOnlyList<UserHistoryResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserHistoryResponseDTO>>> GetUserHistory(int userInformationId)
    {
        var result = await _userHistoryService.GetUserHistoryAsync(userInformationId);
        return Ok(result);
    }

    // GET: api/userhistory/user/{userInformationId}/date/{date}
    [HttpGet("user/{userInformationId}/date/{date}")]
    [ProducesResponseType(typeof(IReadOnlyList<UserHistoryResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<UserHistoryResponseDTO>>> GetUserHistoryByDate(
        int userInformationId, 
        string date)
    {
        // Парсим дату из строки (формат: yyyy-MM-dd)
        if (!DateTime.TryParse(date, out var parsedDate))
        {
            throw new ArgumentException("Неверный формат даты.  Используйте yyyy-MM-dd");
        }

        var result = await _userHistoryService.GetUserHistoryByDateAsync(userInformationId, parsedDate);
        return Ok(result);
    }

    // DELETE: api/userhistory/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserHistoryRecord(int id)
    {
        await _userHistoryService. RemoveUserHistoryRecordAsync(id);
        return NoContent();
    }
}
