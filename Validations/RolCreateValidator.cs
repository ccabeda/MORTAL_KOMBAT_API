using API_MortalKombat.Models.DTOs.RolDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class RolCreateValidator : AbstractValidator<RolCreateDto>
    {
        public RolCreateValidator() 
        {
            RuleFor(r => r.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
        }
    }
}
