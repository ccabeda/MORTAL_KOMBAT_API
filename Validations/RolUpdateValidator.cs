using API_MortalKombat.Models.DTOs.RolDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class RolUpdateValidator : AbstractValidator<RolUpdateDto>
    {
        public RolUpdateValidator()
        {
            RuleFor(r => r.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(r => r.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
        }
    }
}
