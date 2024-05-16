using Microsoft.EntityFrameworkCore;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Implementations;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Evita bucles relacionados con las propiedades de navegaci�n de las entidades.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configuracion de la base de datos
var connectionString = builder.Configuration.GetConnectionString("SystemGymDBConnectionString");
builder.Services.AddDbContext<SystemContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//inyeccion de dependencias
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
//builder.Services.AddScoped<IClassService, ClassService>();
//builder.Services.AddScoped<IClientService, ClientService>();
//builder.Services.AddScoped<ITrainerService, TrainerService>();
//builder.Services.AddScoped<IReserveService, ReserveService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
