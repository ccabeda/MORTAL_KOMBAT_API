using API_MortalKombat.Models.DTOs.PersonajeDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class PersonajeCreateValidator : AbstractValidator<PersonajeCreateDto>
    {
        public PersonajeCreateValidator() 
        {
            RuleFor(n => n.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(n => n.Alineacion).NotEmpty().WithMessage("La alineación no puede estar vacia.");
            RuleFor(n => n.Raza).NotEmpty().WithMessage("La raza no puede estar vacia.");
            RuleFor(n => n.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacia.");
            RuleFor(n => n.ClanId).NotEmpty().WithMessage("El Clan necesita un Id valido.");
            RuleFor(n => n.ReinoId).NotEmpty().WithMessage("El Reino necesita un Id valido.");
        }
    }
}
