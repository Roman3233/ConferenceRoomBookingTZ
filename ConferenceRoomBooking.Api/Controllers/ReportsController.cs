using System;
using System.Linq;
using ConferenceRoomBooking.Api.DTOs.Reports;
using ConferenceRoomBooking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("revenue")]
    public IActionResult GetRevenueReport([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("Start date must be before or equal to end date.");
        }

        var (total, rooms, services) = _reportService.GetRevenueReport(startDate, endDate);
        
        return Ok(new RevenueReportResponse
        {
            TotalRevenue = total,
            RoomRevenue = rooms,
            ServiceRevenue = services
        });
    }
}
