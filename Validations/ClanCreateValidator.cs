using API_MortalKombat.Models.DTOs.ClanDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class ClanCreateValidator : AbstractValidator<ClanCreateDto>
    {
        public ClanCreateValidator() 
        {
            RuleFor(c => c.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(c => c.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
