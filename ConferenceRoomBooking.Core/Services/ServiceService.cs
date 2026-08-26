using ConferenceRoomBooking.Core.Models;
using ConferenceRoomBooking.Core.Interfaces;

namespace ConferenceRoomBooking.Core.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public Service CreateService(string name, decimal price)
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price
        };
        _serviceRepository.Add(service);
        return service;
    }

    public void DeleteService(Guid serviceId)
{
        var deleted = _serviceRepository.Delete(serviceId);
        if (!deleted)
        {
            throw new InvalidOperationException($"Service with id {serviceId} not found.");
        }
    }

    public IEnumerable<Service> GetAllServices()
    {
        return _serviceRepository.GetAll();
    }

    public Service? GetServiceById(Guid serviceId)
    {
        return _serviceRepository.GetById(serviceId);
    }
}