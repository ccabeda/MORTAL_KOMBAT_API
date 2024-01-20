using API_MortalKombat.Models.DTOs.ArmaDTO;
using FluentValidation;


namespace API_MortalKombat.Validations
{
    public class ArmaUpdateValidator : AbstractValidator<ArmaUpdateDto>
    {
        public ArmaUpdateValidator()
        {
            RuleFor(a => a.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(a => a.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(a => a.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
