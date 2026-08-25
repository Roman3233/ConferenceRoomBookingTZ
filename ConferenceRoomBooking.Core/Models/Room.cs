namespace ConferenceRoomBooking.Core.Models;

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePricePerHour { get; set; }
    public List<Guid> AvailableServiceIds { get; set; } = new();
}