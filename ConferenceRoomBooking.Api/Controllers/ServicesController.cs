using ConferenceRoomBooking.Api.DTOs.Services;
using ConferenceRoomBooking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpPost]
    public IActionResult CreateService([FromBody] CreateServiceRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var service = _serviceService.CreateService(request.Name, request.Price);

        var response = new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Price = service.Price
        };
        return CreatedAtAction(nameof(GetServiceById), new { id = response.Id }, response);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteService(Guid id)
    {
        _serviceService.DeleteService(id);
        return NoContent();
    }

    [HttpGet]
    public IActionResult GetAllServices()
    {
        var services = _serviceService.GetAllServices();
        return Ok(services.Select(service => new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Price = service.Price
        }));
    }

    [HttpGet("{id}")]
    public IActionResult GetServiceById(Guid id)
    {
        var service = _serviceService.GetServiceById(id);
        if (service is null)
        {
            return NotFound();
        }

        return Ok(new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Price = service.Price
        });
    }
}