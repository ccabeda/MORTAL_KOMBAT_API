using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class EstiloDePeleaUpdateValidator : AbstractValidator<EstiloDePeleaUpdateDto>
    {
        public EstiloDePeleaUpdateValidator()
        {
            RuleFor(e => e.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(e => e.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(e => e.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
