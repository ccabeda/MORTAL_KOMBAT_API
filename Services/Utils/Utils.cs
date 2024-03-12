using API_MortalKombat.Models;
using FluentValidation;
using System.Net;

namespace API_MortalKombat.Services.Utils
{
    public static class Utils
    {
        public static async Task<APIResponse?> FluentValidator<T>(T dto, IValidator<T> validator, APIResponse apiresponse, ILogger logger)
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
    }
}

