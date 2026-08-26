using ConferenceRoomBooking.Api.DTOs.Rooms;
using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    /// <summary>
    /// Створює нову кімнату.
    /// </summary>
    /// <param name="request">Дані для створення кімнати.</param>
    /// <returns>Створена кімната з її ID та повною вартістю.</returns>
    /// <response code="201">Кімната успішно створена.</response>
    /// <response code="400">Некоректні дані запиту.</response>
    [HttpPost]
    public IActionResult CreateRoom([FromBody] CreateRoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var room = _roomService.CreateRoom(request.Name, request.Capacity, request.BasePricePerHour, request.ServiceIds);

        var response = MapToResponse(room);
        return CreatedAtAction(nameof(GetRoomById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Отримує кімнату за її ID.
    /// </summary>
    /// <param name="id">ID кімнати.</param>
    /// <returns>Об'єкт кімнати.</returns>
    /// <response code="200">Кімната успішно знайдена.</response>
    /// <response code="404">Кімната не знайдена.</response>
    [HttpGet("{id}")]
    public IActionResult GetRoomById(Guid id)
    {
        var room = _roomService.GetRoomById(id);
        if (room is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(room));
    }

    /// <summary>
    /// Отримує всі кімнати.
    /// </summary>
    /// <returns>Колекція об'єктів кімнат.</returns>
    /// <response code="200">Кімнати успішно отримані.</response>
    [HttpGet]
    public IActionResult GetAllRooms()
    {
        var rooms = _roomService.GetAllRooms();
        return Ok(rooms.Select(MapToResponse));
    }

    /// <summary>
    /// Оновлює кімнату за її ID.
    /// </summary>
    /// <param name="id">ID кімнати.</param>
    /// <param name="request">Дані для оновлення кімнати.</param>
    /// <returns>Статус виконання операції.</returns>
    /// <response code="204">Кімната успішно оновлена.</response>
    /// <response code="400">Некоректні дані запиту.</response>
    /// <response code="404">Кімната не знайдена.</response>
    [HttpPut("{id}")]
    public IActionResult UpdateRoom(Guid id, [FromBody] UpdateRoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _roomService.UpdateRoom(id, request.Name, request.Capacity, request.BasePricePerHour, request.ServiceIds);
        return NoContent();
    }

    /// <summary>
    /// Видаляє кімнату за її ID.
    /// </summary>
    /// <param name="id">ID кімнати.</param>
    /// <returns>Статус виконання операції.</returns>
    /// <response code="204">Кімната успішно видалена.</response>
    /// <response code="404">Кімната не знайдена.</response>
    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(Guid id)
    {
        _roomService.DeleteRoom(id);
        return NoContent();
    }

    /// <summary>
    /// Знаходить доступні кімнати на заданий період.
    /// </summary>
    /// <param name="date">Дата.</param>
    /// <param name="startTime">Час початку (включно).</param>
    /// <param name="endTime">Час закінчення (не включно).</param>
    /// <param name="minCapacity">Мінімальна місткість.</param>
    /// <returns>Колекція об'єктів доступних кімнат.</returns>
    /// <response code="200">Доступні кімнати успішно знайдені.</response>
    /// <response code="400">Некоректні дані запиту.</response>
    /// <response code="409">startTime > endTime</response>
    [HttpGet("available")]
    public IActionResult FindAvailableRooms([FromQuery] DateOnly date, [FromQuery] TimeOnly startTime, [FromQuery] TimeOnly endTime, [FromQuery] int minCapacity)
    {
        if (startTime >= endTime)
        {
            return Conflict("Start time must be before end time.");
        }
        
        var rooms = _roomService.FindAvailableRooms(date, startTime, endTime, minCapacity);
        return Ok(rooms.Select(MapToResponse));
    }

    private static RoomResponse MapToResponse(Room room)
    {
        return new RoomResponse
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            BasePricePerHour = room.BasePricePerHour,
            ServiceIds = room.AvailableServiceIds
        };
    }
}