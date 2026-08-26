using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Core.Interfaces;

public interface IServiceService
{
    Service CreateService(string name, decimal price);
    Service? GetServiceById(Guid serviceId);
    void DeleteService(Guid serviceId);
    IEnumerable<Service> GetAllServices();
}