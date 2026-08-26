namespace ConferenceRoomBooking.Api.DTOs.Rooms;

public class RoomResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePricePerHour { get; set; }
    public List<Guid> ServiceIds { get; set; } = new();
}