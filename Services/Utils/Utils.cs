using API_MortalKombat.Data;
using API_MortalKombat.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace API_MortalKombat.Services.Utils
{
    public static class Utils //codigo repetido que se usa mucho en los controller y services.
    {
        public static ActionResult<APIResponse> ControllerHelper(APIResponse apiresponse) //funcion para los controllers
        {
            switch (apiresponse.statusCode)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(apiresponse);
                case HttpStatusCode.Created:
                    return new OkObjectResult(apiresponse);
                case HttpStatusCode.Conflict:
                    return new ConflictObjectResult(apiresponse);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(apiresponse);
                default:
                    return new NotFoundObjectResult(apiresponse);
            }
        }

        public async static Task<APIResponse?> FluentValidator<T>(T dto, IValidator<T> validator, APIResponse apiresponse, ILogger logger) //funcion para el fluentValidation
        {
            var fluentValidation = await validator.ValidateAsync(dto);
            if (!fluentValidation.IsValid)
            {
                var errors = fluentValidation.Errors.Select(e => e.ErrorMessage).ToList();
                logger.LogError("Error al validar los datos de entrada.");
                apiresponse.isExit = false;
                apiresponse.statusCode = HttpStatusCode.BadRequest;
                apiresponse.ErrorList = errors;
                return apiresponse;
            }
            return null;
        }

        public static APIResponse? CheckIfNull<T>(T model, APIResponse apiresponse, ILogger logger) //funcion para verificar que el obj no sean null
        {
            if (model == null) //verifica (user == null)
            {
                apiresponse.isExit = false;
                apiresponse.statusCode = HttpStatusCode.NotFound;
               logger.LogError("Error con los datos ingresados, verifique que todo sea correcto.");
               return apiresponse;
            }
            return null;
        }

        public static APIResponse? ErrorHandling(Exception ex, APIResponse apiresponse, ILogger logger) //funcion para manejar el catch
        {
            logger.LogError("Ocurrio un error inesperado. Error: " + ex.Message);
            apiresponse.isExit = false;
            apiresponse.statusCode = HttpStatusCode.InternalServerError;
            apiresponse.ErrorList = new List<string> { ex.ToString() }; //lista para mantener el error
            return apiresponse;

        }

        public static string GenerarTokendeLogin(Usuario user, IConfiguration config)
        {
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)); //configurar cosas de creación de token JWT 
            var credencial = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.NombreDeUsuario), //no meter contraseña, ya que es facil de ver lo que trae el token
                new Claim(ClaimTypes.GivenName, user.Nombre),
                new Claim(ClaimTypes.Surname, user.Apellido),
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim(ClaimTypes.Role, user.RolId.ToString()),
            };
            //crear token
            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credencial
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    } 
}

