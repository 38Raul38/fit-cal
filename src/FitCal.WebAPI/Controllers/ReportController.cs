using FitCal.Application.Data.DTO.Request;
using FitCal.Application.Data.DTO.Response;
using FitCal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitCal.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController :  ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    // POST: api/report
    [HttpPost]
    [ProducesResponseType(typeof(ReportResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportResponseDTO>> CreateReport([FromBody] ReportRequestDTO request)
    {
        var result = await _reportService. AddReportAsync(request);
        return CreatedAtAction(nameof(GetReportById), new { id = result.ReportId }, result);
    }

    // GET: api/report/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReportResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportResponseDTO>> GetReportById(int id)
    {
        var result = await _reportService.GetReportByIdAsync(id);
        return Ok(result);
    }

    // GET: api/report/user/{userInformationId}
    [HttpGet("user/{userInformationId}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReportResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReportResponseDTO>>> GetUserReports(int userInformationId)
    {
        var result = await _reportService.GetUserReportsAsync(userInformationId);
        return Ok(result);
    }

    // DELETE: api/report/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReport(int id)
    {
        await _reportService.RemoveReportAsync(id);
        return NoContent();
    }
}