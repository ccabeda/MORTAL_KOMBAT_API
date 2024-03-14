using API_MortalKombat.Automapper;
using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Models.DTOs.ReinoDTO;
using API_MortalKombat.Models.DTOs.RolDTO;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using API_MortalKombat.Repository;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Service;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(); //Nuget de patch
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//configurar swagger para autentication, si usas postman no influye. Configuración sacada de internet para logearse en la interfaz Swagger
builder.Services.AddSwaggerGen(c =>
{
    //Titulo y diseño
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MortalKombat API", Version = "V2" });
    //boton de autorización
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

builder.Services.AddDbContext<AplicationDbContext>(option => //Aquí se establece cómo el contexto de la base de datos se conectará a la base de datos,                                                             
//qué proveedor de base de datos se utilizará y otra configuración relacionada con la conexión.
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

//automapper
builder.Services.AddAutoMapper(typeof(AutomapperConfig));
//APIResponse
builder.Services.AddScoped<APIResponse>();

//autenticación con token
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
builder.Services.AddScoped<IRepositoryGeneric<Usuario>, RepositoryUsuario>();
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
builder.Services.AddScoped<IValidator<RolCreateDto>, RolCreateValidator>();
builder.Services.AddScoped<IValidator<RolUpdateDto>, RolUpdateValidator>();

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

