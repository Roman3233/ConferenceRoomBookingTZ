using System.ComponentModel.DataAnnotations;

namespace ConferenceRoomBooking.Api.DTOs.Rooms;

public class CreateRoomRequest
{
    [Required(ErrorMessage = "Room name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Room name must be between 1 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000.")]
    public int Capacity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Base price must be greater than zero.")]
    public decimal BasePricePerHour { get; set; }

    public List<Guid> ServiceIds { get; set; } = new();
}