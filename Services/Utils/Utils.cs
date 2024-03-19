using API_MortalKombat.Models;
using AutoMapper;
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
                case HttpStatusCode.InternalServerError:
                    return new NotFoundObjectResult(apiresponse);
                default:
                    return new NotFoundObjectResult(apiresponse);
            }
        }

        public static bool CheckIfNull<T>(T model) //funcion para verificar que el obj no sean null
        {
            if (model == null) //verifica (user == null)
            {
               return true;
            }
            return false;
        }

        public static bool CheckIfLsitIsNull<T>(List<T> model) //funcion para verificar que la lista no este vacia
        {
            if (model.Count == 0) //verifica (list == null)
            {
                return true;
            }
            return false;
        }

        public static APIResponse ErrorHandling(Exception ex, APIResponse apiresponse, ILogger logger) //funcion para manejar el catch
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

        public static bool CheckIfObjectExist<T>(T model) //funcion para verificar si ya existe el nombre
        {
            if (model != null)
            {
                return true;
            }
            return false;
        }

        public static bool CheckIfNameAlreadyExist<T>(T modelOne,dynamic modelTwo) //funcion para verificar si ya existe el nombre en los updates (no usar anterior porque si
                                                                                                                            //no cambias el nombre no funciona)
        {
            if (modelOne != modelTwo)
            {
                if (modelOne != null)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool PreventDeletionIfRelatedCharacterExist<T>(T obj,IEnumerable<Personaje> list,int id) //aqui podria usarse el metodo cascada para que se borre todo, pero decidi agergarle esto para mas seguridad
        {
            foreach (var i in list)
            {
                if (i.ClanId == id && obj is Clan || i.ReinoId == id && obj is Reino)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool PreventDeletionIfRelatedUserExist(IEnumerable<Usuario> list, int id) 
        {
            foreach (var i in list)
            {
                if (i.RolId == id)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool VerifyPassword(string password, string passwordEncrypted) //verificar las password en los endpoints
        {
            if (!Encrypt.VerifyPassword(password, passwordEncrypted))
            {
                return false;
            }
            return true;
        }

        public static bool VerifyIfCharacterContains<T>(T obj, Personaje personaje, int id) //funcion para verificar si el personaje contiene o no el objeto
        {
            if (personaje.Armas.Any(a => a.Id == id) && obj is Arma || personaje.EstilosDePeleas.Any(e => e.Id == id) && obj is EstiloDePelea)
            {
                return true;
            }
            return false;
        }

        public static APIResponse BadRequestResponse(APIResponse apiresponse)
        {
            apiresponse.isExit = false;
            apiresponse.statusCode = HttpStatusCode.BadRequest;
            return apiresponse;
        }

        public static APIResponse ConflictResponse(APIResponse apiresponse)
        {
            apiresponse.isExit = false;
            apiresponse.statusCode = HttpStatusCode.Conflict;
            return apiresponse;
        }

        public static APIResponse NotFoundResponse(APIResponse apiresponse)
        {
            apiresponse.isExit = false;
            apiresponse.statusCode = HttpStatusCode.NotFound;
            return apiresponse;
        }

        public static APIResponse OKResponse<T, A>(IMapper mapper, A obj, APIResponse apiresponse) //funcion para repsonder correctamente los getbyAlgo
        {
            apiresponse.Result = mapper.Map<T>(obj);
            apiresponse.statusCode = HttpStatusCode.OK;
            return apiresponse;
        }

        public static APIResponse ListOKResponse<T, A>(IMapper mapper, List<A> objs, APIResponse apiresponse) //funcion para responder correctamente los getAll
        {
            apiresponse.Result = mapper.Map<IEnumerable<T>>(objs);
            apiresponse.statusCode = HttpStatusCode.OK;
            return apiresponse;
        }

        //public async static Task<APIResponse?> FluentValidator<T>(T dto, IValidator<T> validator, APIResponse apiresponse, ILogger logger) //funcion para el fluentValidation. Ahora con el nugget se hace automatico.
        //{
        //    var fluentValidation = await validator.ValidateAsync(dto);
        //    if (!fluentValidation.IsValid)
        //    {
        //        var errors = fluentValidation.Errors.Select(e => e.ErrorMessage).ToList();
        //        logger.LogError("Error al validar los datos de entrada.");
        //        apiresponse.isExit = false;
        //        apiresponse.statusCode = HttpStatusCode.BadRequest;
        //        apiresponse.ErrorList = errors;
        //        return apiresponse;
        //    }
        //    return null;
        //}
    }
}