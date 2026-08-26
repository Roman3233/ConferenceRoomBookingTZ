using System;
using System.Collections.Generic;
using ConferenceRoomBooking.Core.Models;
using ConferenceRoomBooking.Core.Services;
using ConferenceRoomBooking.Infrastructure.Repositories;
using Xunit;

namespace ConferenceRoomBooking.Tests;

public class BookingServiceTests
{
    private readonly InMemoryBookingRepository _bookingRepository;
    private readonly InMemoryRoomRepository _roomRepository;
    private readonly InMemoryServiceRepository _serviceRepository;
    private readonly BookingAvailabilityChecker _availabilityChecker;
    private readonly PricingCalculator _pricingCalculator;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _bookingRepository = new InMemoryBookingRepository();
        _roomRepository = new InMemoryRoomRepository();
        _serviceRepository = new InMemoryServiceRepository();
        _availabilityChecker = new BookingAvailabilityChecker(_bookingRepository);
        _pricingCalculator = new PricingCalculator();

        _bookingService = new BookingService(
            _bookingRepository,
            _roomRepository,
            _pricingCalculator,
            _serviceRepository,
            _availabilityChecker
        );
    }

    [Fact]
    public void CreateBooking_Successful_CalculatesCorrectPriceAndSaves()
    {
        // Arrange
        var room = new Room
        {
            Name = "Room A",
            Capacity = 10,
            BasePricePerHour = 1000m
        };
        _roomRepository.Add(room);

        var date = new DateOnly(2026, 8, 26);
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(11, 0); // 2 години стандарту = 2000m

        // Act
        var booking = _bookingService.CreateBooking(room.Id, date, start, end, new List<Guid>());

        // Assert
        Assert.NotNull(booking);
        Assert.Equal(2000m, booking.TotalPrice);
        Assert.Equal(2000m, booking.RoomCostAtBooking);
        
        // Перевіряємо, що запис дійсно зберігся в репозиторій
        var savedBooking = _bookingRepository.GetById(booking.Id);
        Assert.NotNull(savedBooking);
        Assert.Equal(room.Id, savedBooking.RoomId);
    }

    [Fact]
    public void CreateBooking_ConflictingTime_ThrowsInvalidOperationException()
    {
        // Arrange
        var room = new Room
        {
            Name = "Room A",
            Capacity = 10,
            BasePricePerHour = 1000m
        };
        _roomRepository.Add(room);

        var date = new DateOnly(2026, 8, 26);
        
        // 1. Створюємо перше бронювання на 10:00 - 12:00
        _bookingService.CreateBooking(room.Id, date, new TimeOnly(10, 0), new TimeOnly(12, 0), new List<Guid>());

        // 2. Act & Assert: спроба забронювати на перетинаючийся час (11:00 - 13:00) має викликати виключення
        Assert.Throws<InvalidOperationException>(() =>
            _bookingService.CreateBooking(room.Id, date, new TimeOnly(11, 0), new TimeOnly(13, 0), new List<Guid>())
        );
    }
}