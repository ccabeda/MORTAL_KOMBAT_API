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

//configurar swagger para autentication, si usas postman no influye. Configuraci�n sacada de internet para logearse en la interfaz Swagger
builder.Services.AddSwaggerGen(c =>
{
    //Titulo y dise�o
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Village API", Version = "V2" });
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

builder.Services.AddDbContext<AplicationDbContext>(option => //Aqu� se establece c�mo el contexto de la base de datos se conectar� a la base de datos,                                                             //qu� proveedor de base de datos se utilizar� y otra configuraci�n relacionada con la conexi�n.
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
//repository
builder.Services.AddScoped<IRepositoryPersonaje, RepositoryPersonaje>();
builder.Services.AddScoped<IRepositoryClan, RepositoryClan>();
builder.Services.AddScoped<IRepositoryReino, RepositoryReino>();
builder.Services.AddScoped<IRepositoryArma, RepositoryArma>();
builder.Services.AddScoped<IRepositoryEstiloDePelea, RepositoryEstiloDePelea>();
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddScoped<IRepositoryRol,  RepositoryRol>();
builder.Services.AddScoped<IRepositoryLogin, RepositoryLogin>();
//service
builder.Services.AddScoped<IServicePersonaje,ServicePersonaje>();
builder.Services.AddScoped<IServiceClan, ServiceClan>();
builder.Services.AddScoped<IServiceReino, ServiceReino>();
builder.Services.AddScoped<IServiceArma, ServiceArma>();
builder.Services.AddScoped<IServiceEstiloDePelea, ServiceEstiloDePelea>();
builder.Services.AddScoped<IServiceUsuario, ServiceUsuario>();
builder.Services.AddScoped<IServiceRol, ServiceRol>();
builder.Services.AddScoped<IServiceLogin, ServiceLogin>();
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

