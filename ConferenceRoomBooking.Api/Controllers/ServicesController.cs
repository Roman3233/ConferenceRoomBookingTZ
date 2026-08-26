using System;
using System.Linq;
using ConferenceRoomBooking.Api.DTOs.Services;
using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;
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
        var response = MapToResponse(service);

        return CreatedAtAction(nameof(GetServiceById), new { id = response.Id }, response);
    }

    [HttpGet("{id}")]
    public IActionResult GetServiceById(Guid id)
    {
        var service = _serviceService.GetServiceById(id);
        if (service is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(service));
    }

    [HttpGet]
    public IActionResult GetAllServices()
    {
        var services = _serviceService.GetAllServices();
        return Ok(services.Select(MapToResponse));
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteService(Guid id)
    {
        try
        {
            _serviceService.DeleteService(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    private static ServiceResponse MapToResponse(Service service)
    {
        return new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Price = service.Price
        };
    }
}