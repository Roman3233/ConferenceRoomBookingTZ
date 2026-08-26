using ConferenceRoomBooking.Api.DTOs.Bookings;
using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;


namespace ConferenceRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public IActionResult CreateBooking([FromBody] CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var booking = _bookingService.CreateBooking(
            request.RoomId,
            request.Date,
            request.StartTime,
            request.EndTime,
            request.ServiceIds
        );

        var response = MapToResponse(booking);
        return CreatedAtAction(nameof(GetBookingById), new { id = response.Id }, response);
    }

    [HttpGet("{id}")]
    public IActionResult GetBookingById(Guid id)
    {
        var booking = _bookingService.GetBookingById(id);
        if (booking is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(booking));
    }

    [HttpGet]
    public IActionResult GetAllBookings()
    {
        var bookings = _bookingService.GetAllBookings();
        return Ok(bookings.Select(MapToResponse));
    }
    private static BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            Date = booking.Date,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            RoomCostAtBooking = booking.RoomCostAtBooking,
            Services = booking.Services.Select(x => new BookedServiceResponse
            {
                ServiceId = x.ServiceId,
                ServiceName = x.ServiceName,
                PriceAtBooking = x.PriceAtBooking
            }).ToList(),
            TotalPrice = booking.TotalPrice
        };
    }
}