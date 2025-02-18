using HotelManagement.Application.Data;
using HotelManagement.Application.Services;
using HotelManagement.Domain.Interfaces;
using HotelManagement.Infraestructure.Repositories;
using HotelManagement.Infraestructure.Utilities;
using Microsoft.EntityFrameworkCore;
using static HotelManagement.Application.Dtos.EmailDto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<HotelManagementService>();
builder.Services.AddScoped<IHotelManagement, HotelManagementRepository>();

builder.Services.AddScoped<HotelReservationService>();
builder.Services.AddScoped<IHotelReservation, HotelReservationRepository>();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddSingleton<EmailRepository>();

builder.Services.AddScoped<IEmail, EmailRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); 
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
