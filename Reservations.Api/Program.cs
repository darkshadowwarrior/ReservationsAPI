using Reservations.Api.Managers;
using Reservations.Api.Repositories;
using Reservations.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IPricingManager, PricingManager>();
builder.Services.AddSingleton<IReservationsRepository, ReservationsRepository>();
builder.Services.AddSingleton<IParkingSpaceManager, ParkingSpaceManager>();
builder.Services.AddSingleton<IReservationManager, ReservationManager>();
builder.Services.AddSingleton<IReservationService, ReservationService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
