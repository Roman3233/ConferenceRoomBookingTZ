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
    
    /// <summary>
    /// Отримання звіту про доходи.
    /// </summary>
    /// <param name="startDate">Дата початку періоду (включно).</param>
    /// <param name="endDate">Дата закінчення періоду (включно).</param>
    /// <returns>Об'єкт з інформацією про загальний дохід, дохід від залів та послуг.</returns>
    /// <response code="200">Звіт успішно згенеровано.</response>
    /// <response code="400">Некоректний діапазон дат (дата початку пізніша за дату закінчення).</response>
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
    
    /// <summary>
    /// Отримання звіту про завантаженість залів за певний період.
    /// </summary>
    /// <param name="startDate">Дата початку періоду (включно).</param>
    /// <param name="endDate">Дата закінчення періоду (включно).</param>
    /// <returns>Колекція об'єктів, що показують ID залу, назву, загальну кількість заброньованих годин та відсоток завантаженості.</returns>
    /// <response code="200">Звіт успішно згенеровано.</response>
    /// <response code="400">Некоректний діапазон дат (дата початку пізніша за дату закінчення).</response>
    [HttpGet("occupancy")]
    public IActionResult GetOccupancyReport([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("Start date must be before or equal to end date.");
        }

        var report = _reportService.GetOccupancyReport(startDate, endDate);
        var response = report.Select(r => new RoomOccupancyResponse
        {
            RoomId = r.RoomId,
            RoomName = r.RoomName,
            TotalHoursBooked = r.TotalHoursBooked,
            OccupancyRatePercentage = r.OccupancyRate
        });

        return Ok(response);
    }
}
