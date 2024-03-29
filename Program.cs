using API_MortalKombat.Automapper;
using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using API_MortalKombat.Models.DTOs.ReinoDTO;
using API_MortalKombat.Models.DTOs.RolDTO;
using API_MortalKombat.Repository;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Service;
using API_MortalKombat.Services.IService;
using API_MortalKombat.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; //para ignorar ciclos
}); //Nuget de patch


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//configurar swagger para autentication, si usas postman no influye. Configuraci�n sacada de internet para logearse en la interfaz Swagger
builder.Services.AddSwaggerGen(c =>
{
    //Titulo y dise�o
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MortalKombat API", Version = "V2" });
    //boton de autorizaci�n
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
      }
    });
});

builder.Services.AddDbContext<AplicationDbContext>(option => //Aqu� se establece c�mo el contexto de la base de datos se conectar� a la base de datos,                                                             
//qu� proveedor de base de datos se utilizar� y otra configuraci�n relacionada con la conexi�n.
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

//automapper
builder.Services.AddAutoMapper(typeof(AutomapperConfig));
//APIResponse
builder.Services.AddScoped<APIResponse>();

//autenticaci�n con token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
//repository
builder.Services.AddScoped<IRepositoryGeneric<Personaje>, RepositoryPersonaje>();
builder.Services.AddScoped<IRepositoryGeneric<Clan>, RepositoryClan>();
builder.Services.AddScoped<IRepositoryGeneric<Reino>, RepositoryReino>();
builder.Services.AddScoped<IRepositoryGeneric<Arma>, RepositoryArma>();
builder.Services.AddScoped<IRepositoryGeneric<EstiloDePelea>, RepositoryEstiloDePelea>();
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddScoped<IRepositoryGeneric<Rol>,  RepositoryRol>();
//service
builder.Services.AddScoped<IServiceGeneric<ClanUpdateDto, ClanCreateDto>, ServiceClan>();
builder.Services.AddScoped<IServiceGeneric<ReinoUpdateDto, ReinoCreateDto>, ServiceReino>();
builder.Services.AddScoped<IServiceGeneric<ArmaUpdateDto, ArmaCreateDto>, ServiceArma>();
builder.Services.AddScoped<IServiceGeneric<EstiloDePeleaUpdateDto, EstiloDePeleaCreateDto>, ServiceEstiloDePelea>();
builder.Services.AddScoped<IServiceGeneric<RolUpdateDto, RolCreateDto>, ServiceRol>();
builder.Services.AddScoped<IServiceLogin, ServiceLogin>();
builder.Services.AddScoped<IServiceUsuario, ServiceUsuario>();
builder.Services.AddScoped<IServicePersonaje, ServicePersonaje>();
//unitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<IValidator<PersonajeCreateDto>, PersonajeCreateValidator>();
//Fluent validation. COn el Uso de un nugget, se autoconfiguran todos los fluentValidation (a mano seria como arriba). Ayuda muchisimo.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

