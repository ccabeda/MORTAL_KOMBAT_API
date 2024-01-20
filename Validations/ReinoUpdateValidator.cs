using API_MortalKombat.Models.DTOs.ReinoDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class ReinoUpdateValidator : AbstractValidator<ReinoUpdateDto>
    {
        public ReinoUpdateValidator()
        {
            RuleFor(r => r.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(r => r.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(r => r.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
