using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class EstiloDePeleaCreateValidator : AbstractValidator<EstiloDePeleaCreateDto>
    {
        public EstiloDePeleaCreateValidator()
        {
            RuleFor(n => n.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(n => n.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
