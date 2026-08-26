using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Services;
using ConferenceRoomBooking.Infrastructure.Repositories;
using ConferenceRoomBooking.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRoomRepository, InMemoryRoomRepository>();
builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddSingleton<IServiceRepository, InMemoryServiceRepository>();

builder.Services.AddSingleton<IBookingAvailabilityChecker, BookingAvailabilityChecker>();
builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<IBookingService, BookingService>();
builder.Services.AddSingleton<IServiceService, ServiceService>();
builder.Services.AddSingleton<IReportService, ReportService>();
builder.Services.AddSingleton<IPricingCalculator, PricingCalculator>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Заповнюємо сховище початковими даними при старті застосунку.
using (var scope = app.Services.CreateScope())
{
    var serviceRepository = scope.ServiceProvider.GetRequiredService<IServiceRepository>();
    var roomRepository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
    ConferenceRoomBooking.Infrastructure.DataSeeder.SeedInitialData(serviceRepository, roomRepository);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();