using API_MortalKombat.Models.DTOs.ArmaDTO;
using FluentValidation;

namespace MiPrimeraAPI.Validations
{
    public class ArmaCreateValidator : AbstractValidator<ArmaCreateDto>
    {
        public ArmaCreateValidator() 
        {
            RuleFor(n => n.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(n => n.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
