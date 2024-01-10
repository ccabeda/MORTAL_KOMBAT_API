using API_MortalKombat.Models.DTOs.ArmaDTO;
using FluentValidation;


namespace API_MortalKombat.Validations
{
    public class ArmaUpdateValidator : AbstractValidator<ArmaUpdateDto>
    {
        public ArmaUpdateValidator()
        {
            RuleFor(i => i.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(n => n.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(n => n.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}
