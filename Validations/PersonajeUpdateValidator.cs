using API_MortalKombat.Models.DTOs.PersonajeDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class PersonajeUpdateValidator : AbstractValidator<PersonajeUpdateDto>
    {
        public PersonajeUpdateValidator()
        {
            RuleFor(p => p.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(p => p.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(p => p.Alineacion).NotEmpty().WithMessage("La alineación no puede estar vacia.");
            RuleFor(p => p.Raza).NotEmpty().WithMessage("La raza no puede estar vacia.");
            RuleFor(p => p.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacia.");
            RuleFor(p => p.ClanId).NotEmpty().WithMessage("El Clan necesita un Id valido.");
            RuleFor(p => p.ReinoId).NotEmpty().WithMessage("El Reino necesita un Id valido.");
        }
    }
}
