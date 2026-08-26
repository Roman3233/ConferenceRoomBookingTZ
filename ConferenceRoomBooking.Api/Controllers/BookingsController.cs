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
    
    /// <summary>
    /// Створення нового бронювання.
    /// </summary>
    /// <param name="request">Об'єкт з даними про бронювання.</param>
    /// <returns>Новостворене бронювання з його ID та повною вартістю.</returns>
    /// <response code="201">Бронювання успішно створено.</response>
    /// <response code="400">Некоректні дані запиту (невалідна модель або відсутність вільного залу).</response>
    /// <response code="404">Зал не існує.</response>
    /// <response code="409">Зал не вільний у цей час</response>
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

    /// <summary>
    /// Отримання бронювання за ID.
    /// </summary>
    /// <param name="id">ID бронювання.</param>
    /// <returns>Об'єкт бронювання.</returns>
    /// <response code="200">Бронювання успішно отримано.</response>
    /// <response code="404">Бронювання не знайдено.</response>
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

    /// <summary>
    /// Отримання всіх бронювань.
    /// </summary>
    /// <returns>Колекція об'єктів бронювань.</returns>
    /// <response code="200">Бронювання успішно отримано.</response>
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