using System.ComponentModel.DataAnnotations;

namespace ConferenceRoomBooking.Api.DTOs.Bookings;

public class CreateBookingRequest
{
    [Required(ErrorMessage = "Room id is required.")]
    public Guid RoomId { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    public DateOnly Date { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    public TimeOnly StartTime { get; set; }

    [Required(ErrorMessage = "End time is required.")]
    public TimeOnly EndTime { get; set; }

    public List<Guid> ServiceIds { get; set; } = new();
}