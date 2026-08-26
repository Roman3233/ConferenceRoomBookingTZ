namespace ConferenceRoomBooking.Api.DTOs.Services;

public class ServiceResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}