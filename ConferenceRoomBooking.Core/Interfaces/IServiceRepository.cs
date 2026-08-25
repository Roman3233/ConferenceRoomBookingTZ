using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Core.Interfaces;

public interface IServiceRepository
{
    Service? GetById(Guid id);
    IEnumerable<Service> GetAll();
    Service Add(Service service);
    void Update(Service service);
    bool Delete(Guid id);
}