namespace ConferenceRoomBooking.Api.DTOs.Reports;

public class RoomOccupancyResponse
{
    public Guid RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public double TotalHoursBooked { get; set; }
    public double OccupancyRatePercentage { get; set; }
}
