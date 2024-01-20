using API_MortalKombat;
using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Models.DTOs.ReinoDTO;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using API_MortalKombat.Repository;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Service;
using API_MortalKombat.Service.IService;
using API_MortalKombat.Services;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AplicationDbContext>(option => //Aquí se establece cómo el contexto de la base de datos se conectará a la base de datos,                                                             //qué proveedor de base de datos se utilizará y otra configuración relacionada con la conexión.
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
builder.Services.AddScoped<IRepositoryReino, RepositoryReino>();
builder.Services.AddScoped<IRepositoryArma, RepositoryArma>();
builder.Services.AddScoped<IRepositoryEstiloDePelea, RepositoryEstiloDePelea>();
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddScoped<IRepositoryRol,  RepositoryRol>();
//service
builder.Services.AddScoped<IServicePersonaje,ServicePersonaje>();
builder.Services.AddScoped<IServiceClan, ServiceClan>();
builder.Services.AddScoped<IServiceReino, ServiceReino>();
builder.Services.AddScoped<IServiceArma, ServiceArma>();
builder.Services.AddScoped<IServiceEstiloDePelea, ServiceEstiloDePelea>();
builder.Services.AddScoped<IServiceUsuario, ServiceUsuario>();
builder.Services.AddScoped<IServiceRol, ServiceRol>();
//fluent validation
builder.Services.AddScoped<IValidator<PersonajeCreateDto>, PersonajeCreateValidator>();
builder.Services.AddScoped<IValidator<PersonajeUpdateDto>, PersonajeUpdateValidator>();
builder.Services.AddScoped<IValidator<ClanCreateDto>, ClanCreateValidator>();
builder.Services.AddScoped<IValidator<ClanUpdateDto>, ClanUpdateValidator>();
builder.Services.AddScoped<IValidator<ReinoCreateDto>, ReinoCreateValidator>();
builder.Services.AddScoped<IValidator<ReinoUpdateDto>, ReinoUpdateValidator>();
builder.Services.AddScoped<IValidator<ArmaCreateDto>, ArmaCreateValidator>();
builder.Services.AddScoped<IValidator<ArmaUpdateDto>, ArmaUpdateValidator>();
builder.Services.AddScoped<IValidator<EstiloDePeleaCreateDto>, EstiloDePeleaCreateValidator>();
builder.Services.AddScoped<IValidator<EstiloDePeleaUpdateDto>, EstiloDePeleaUpdateValidator>();
builder.Services.AddScoped<IValidator<UsuarioCreateDto>, UsuarioCreateValidator>();
builder.Services.AddScoped<IValidator<UsuarioUpdateDto>, UsuarioUpdateValidator>();

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

