using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Infrastructure;

// Заповнює сховище початковими даними при старті застосунку.
public static class DataSeeder
{
    public static void SeedInitialData(IServiceRepository serviceRepository, IRoomRepository roomRepository)
    {
        var projector = serviceRepository.Add(new Service { Name = "Проєктор", Price = 500m });
        var wifi = serviceRepository.Add(new Service { Name = "Wi-Fi", Price = 300m });
        var sound = serviceRepository.Add(new Service { Name = "Звук", Price = 700m });

        roomRepository.Add(new Room
        {
            Name = "Зал А",
            Capacity = 50,
            BasePricePerHour = 2000m,
            AvailableServiceIds = [projector.Id, wifi.Id, sound.Id]
        });

        roomRepository.Add(new Room
        {
            Name = "Зал B",
            Capacity = 100,
            BasePricePerHour = 3500m,
            AvailableServiceIds = [projector.Id, wifi.Id, sound.Id]
        });

        roomRepository.Add(new Room
        {
            Name = "Зал C",
            Capacity = 30,
            BasePricePerHour = 1500m,
            AvailableServiceIds = [projector.Id, wifi.Id, sound.Id]
        });
    }
}