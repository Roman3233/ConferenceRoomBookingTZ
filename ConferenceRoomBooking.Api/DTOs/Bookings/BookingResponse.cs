namespace ConferenceRoomBooking.Api.DTOs.Bookings;

public class BookingResponse
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal RoomCostAtBooking { get; set; }
    public List<BookedServiceResponse> Services { get; set; } = new();
    public decimal TotalPrice { get; set; }
}

public class BookedServiceResponse
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal PriceAtBooking { get; set; }
}