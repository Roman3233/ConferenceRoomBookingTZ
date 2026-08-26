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

    [HttpGet]
    public IActionResult GetAllRooms()
    {
        var rooms = _roomService.GetAllRooms();
        return Ok(rooms.Select(MapToResponse));
    }

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

    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(Guid id)
    {
        _roomService.DeleteRoom(id);
        return NoContent();
    }

    [HttpGet("available")]
    public IActionResult FindAvailableRooms([FromQuery] DateOnly date, [FromQuery] TimeOnly startTime, [FromQuery] TimeOnly endTime, [FromQuery] int minCapacity)
    {
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