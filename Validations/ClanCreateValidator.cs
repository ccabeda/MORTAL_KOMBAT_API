using FluentValidation;
using MortalKombat_API.Models.DTOs.ClanDTO;

namespace MiPrimeraAPI.Validations
{
    public class ClanCreateValidator : AbstractValidator<ClanCreateDto>
    {
        public ClanCreateValidator() 
        {
            RuleFor(n => n.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(n => n.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
