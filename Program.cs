using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Repository;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services;
using API_MortalKombat.Services.IServices;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MiPrimeraAPI;
using MiPrimeraAPI.Models;
using MiPrimeraAPI.Validations;
using MortalKombat_API.Data;
using MortalKombat_API.Models.DTOs.ClanDTO;
using MortalKombat_API.Models.DTOs.PersonajeDTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AplicationDbContext>(option => //Aqu� se establece c�mo el contexto de la base de datos se conectar� a la base de datos,
                                                             //qu� proveedor de base de datos se utilizar� y otra configuraci�n relacionada con la conexi�n.
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

//automapper
builder.Services.AddAutoMapper(typeof(AutomapperConfig));
//APIResponse
builder.Services.AddScoped<APIResponse>();
//repository
builder.Services.AddScoped<IRepositoryPersonaje, RepositoryPersonaje>();
builder.Services.AddScoped<IRepositoryClan, RepositoryClan>();
//service
builder.Services.AddScoped<IServicePersonaje,ServicePersonaje>();
builder.Services.AddScoped<IServiceClan, ServiceClan>();
//fluent validation
builder.Services.AddScoped<IValidator<PersonajeCreateDto>, PersonajeCreateValidator>();
builder.Services.AddScoped<IValidator<PersonajeUpdateDto>, PersonajeUpdateValidator>();
builder.Services.AddScoped<IValidator<ClanCreateDto>, ClanCreateValidator>();
builder.Services.AddScoped<IValidator<ClanUpdateDto>, ClanUpdateValidator>();

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

