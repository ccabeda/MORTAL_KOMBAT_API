using FluentValidation;
using MortalKombat_API.Models.DTOs.ReinoDTO;

namespace MiPrimeraAPI.Validations
{
    public class ReinoCreateValidator : AbstractValidator<ReinoCreateDto>
    {
        public ReinoCreateValidator() 
        {
            RuleFor(n => n.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(n => n.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
