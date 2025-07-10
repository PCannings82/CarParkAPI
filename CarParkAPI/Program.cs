using CarParkAPI.DBContext;
using CarParkAPI.Enums;
using CarParkAPI.Models;
using CarParkAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CarParkDB>(opt =>
    opt.UseInMemoryDatabase("CarParkDB"));

builder.Services.AddScoped<IParkingService, ParkingService>();
builder.Services.AddScoped<ICostService, CostService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CarParkDB>();

    if (!db.Cost.Any())
    {
        db.Cost.Add(new Cost { TotalCollected = 0 });
        await db.SaveChangesAsync();
    }

    if (!db.ParkingSpace.Any())
    {
        for (int i = 1; i <= 50; i++)
        {
            db.ParkingSpace.Add(new ParkingSpace
            {
                Id = i,
                Status = SpaceStatus.Available
            });
        }

        await db.SaveChangesAsync();
    }
}

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
