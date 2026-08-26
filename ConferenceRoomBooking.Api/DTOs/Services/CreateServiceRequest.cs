using System.ComponentModel.DataAnnotations;

namespace ConferenceRoomBooking.Api.DTOs.Services;

public class CreateServiceRequest
{
    [Required(ErrorMessage = "Service name is required.")]
    [MaxLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1,000,000.")]
    public decimal Price { get; set; }
}