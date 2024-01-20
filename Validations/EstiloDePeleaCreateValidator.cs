using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class EstiloDePeleaCreateValidator : AbstractValidator<EstiloDePeleaCreateDto>
    {
        public EstiloDePeleaCreateValidator()
        {
            RuleFor(e => e.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(e => e.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
