using API_MortalKombat.Models.DTOs.ReinoDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
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
