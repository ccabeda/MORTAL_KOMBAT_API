using API_MortalKombat.Models.DTOs.ClanDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class ClanUpdateValidator : AbstractValidator<ClanUpdateDto>
    {
        public ClanUpdateValidator()
        {
            RuleFor(c => c.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(c => c.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(c => c.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
