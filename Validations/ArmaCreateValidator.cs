using API_MortalKombat.Models.DTOs.ArmaDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class ArmaCreateValidator : AbstractValidator<ArmaCreateDto>
    {
        public ArmaCreateValidator() 
        {
            RuleFor(a => a.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(a => a.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
