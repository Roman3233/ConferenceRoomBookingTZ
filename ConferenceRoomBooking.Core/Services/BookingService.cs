using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Core.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IPricingCalculator _pricingCalculator;
    private readonly IServiceRepository _serviceRepository;
    private readonly IBookingAvailabilityChecker _bookingAvailabilityChecker;
    
    public BookingService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IPricingCalculator pricingCalculator,
        IServiceRepository serviceRepository,
        IBookingAvailabilityChecker bookingAvailabilityChecker)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _pricingCalculator = pricingCalculator;
        _serviceRepository = serviceRepository;
        _bookingAvailabilityChecker = bookingAvailabilityChecker;
    }
    
    public Booking CreateBooking(Guid roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime, List<Guid> serviceIds)
    {
        var room = _roomRepository.GetById(roomId);
        if(room is null)
        {
            throw new InvalidOperationException($"Room with id {roomId} not found.");
        }
        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time must be before end time.", nameof(startTime));
        }
        if(_bookingAvailabilityChecker.HasConflictingBooking(roomId, date, startTime, endTime))
        {
            throw new InvalidOperationException("Room is already booked for this time.");
        }

        decimal roomCost  = _pricingCalculator.CalculateRoomCost(room.BasePricePerHour, startTime, endTime);
        decimal servicesCost = 0;

        var bookedServices = new List<BookedService>();
        if (serviceIds is not null && serviceIds.Count > 0)
        {
            foreach (var serviceId in serviceIds)
            {
                var service = _serviceRepository.GetById(serviceId);
                if (service is null || !room.AvailableServiceIds.Contains(serviceId))
                {
                    throw new InvalidOperationException($"Service {serviceId} is not available for room {roomId}.");
                }

                bookedServices.Add(new BookedService
                {
                    ServiceId = serviceId,
                    ServiceName = service.Name,
                    PriceAtBooking = service.Price
                });

                servicesCost += service.Price;
            }
        }

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomId,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            Services = bookedServices,
            RoomCostAtBooking = roomCost,
            TotalPrice = roomCost + servicesCost
        };
        return _bookingRepository.Add(booking);

    }

    public Booking? GetBookingById(Guid bookingId)
    {
        return _bookingRepository.GetById(bookingId);
    }

    public IEnumerable<Booking> GetAllBookings()
    {
        return _bookingRepository.GetAll();
    }
}