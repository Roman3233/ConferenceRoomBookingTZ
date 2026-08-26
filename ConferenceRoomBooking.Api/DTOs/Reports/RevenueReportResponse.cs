namespace ConferenceRoomBooking.Api.DTOs.Reports;

public class RevenueReportResponse
{
    public decimal TotalRevenue { get; set; }
    public decimal RoomRevenue { get; set; }
    public decimal ServiceRevenue { get; set; }
}