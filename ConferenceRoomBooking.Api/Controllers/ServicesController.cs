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
    /// <summary>
    /// Створює нову послугу
    /// </summary>
    /// <param name="request">Дані для створення послуги</param>
    /// <returns>Створена послуга з її ID та повною вартістю</returns>
    /// <response code="201">Послуга успішно створена</response>
    /// <response code="400">Некоректні дані запиту</response>
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
    /// <summary>
    /// Отримує послугу за її ID.
    /// </summary>
    /// <param name="id">ID послуги.</param>
    /// <returns>Об'єкт послуги.</returns>
    /// <response code="200">Послуга успішно знайдена.</response>
    /// <response code="404">Послуга не знайдена.</response>
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
    /// <summary>
    /// Отримує всі послуги.
    /// </summary>
    /// <returns>Колекція об'єктів послуг.</returns>
    /// <response code="200">Послуги успішно отримані.</response>
    [HttpGet]
    public IActionResult GetAllServices()
    {
        var services = _serviceService.GetAllServices();
        return Ok(services.Select(MapToResponse));
    }
    /// <summary>
    /// Видаляє послугу за її ID.
    /// </summary>
    /// <param name="id">ID послуги.</param>
    /// <returns>Статус виконання операції.</returns>
    /// <response code="204">Послуга успішно видалена.</response>
    /// <response code="404">Послуга не знайдена.</response>
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