using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Infrastructure.Repositories;

public class InMemoryServiceRepository : IServiceRepository
{
    private readonly List<Service> _services = new();
    private readonly object _lock = new();

    public Service? GetById(Guid id)
    {
        lock(_lock)
        {
            return _services.FirstOrDefault(s => s.Id == id);
        }
    }

    public IEnumerable<Service> GetAll()
    {
        lock(_lock)
        {
            return _services.ToList();
        }
    }

    public Service Add(Service service)
    {
        lock(_lock)
        {
            service.Id = Guid.NewGuid();
            _services.Add(service);
            return service;
        }
    }

    public void Update(Service service)
    {
        lock(_lock)
        {
            var existing = GetById(service.Id);
            if (existing is null)
            {
                throw new InvalidOperationException($"Service with id {service.Id} not found.");
            }

            _services.Remove(existing);
            _services.Add(service);
        }
    }

    public bool Delete(Guid id)
    {
        lock(_lock)
        {
            var service = GetById(id);
            if (service is null)
            {
                return false;
            }

            return _services.Remove(service);
        }
    }
}