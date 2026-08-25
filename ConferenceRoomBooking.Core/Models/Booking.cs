namespace ConferenceRoomBooking.Core.Models;

public class Booking
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public List<BookedService> Services { get; set; } = new();
    public decimal RoomCostAtBooking { get; set; }

    // Загальна вартість = RoomCostAtBooking + сума цін послуг
    public decimal TotalPrice { get; set; }
}

// Snapshot-клас: яку послугу обрали і за якою ціною на момент бронювання
public class BookedService
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal PriceAtBooking { get; set; }
}