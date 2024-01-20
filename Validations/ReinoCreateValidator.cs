using API_MortalKombat.Models.DTOs.ReinoDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class ReinoCreateValidator : AbstractValidator<ReinoCreateDto>
    {
        public ReinoCreateValidator() 
        {
            RuleFor(r => r.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(r => r.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
