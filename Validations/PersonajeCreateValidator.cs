using FluentValidation;
using MortalKombat_API.Models.DTOs.PersonajeDTO;

namespace MiPrimeraAPI.Validations
{
    public class PersonajeCreateValidator : AbstractValidator<PersonajeCreateDto>
    {
        public PersonajeCreateValidator() 
        {
            RuleFor(n => n.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(n => n.Alineacion).NotEmpty().WithMessage("La alineación no puede estar vacia.");
            RuleFor(n => n.Raza).NotEmpty().WithMessage("La raza no puede estar vacia.");
            RuleFor(n => n.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacia.");
            RuleFor(n => n.EstilosDePelea).NotEmpty().WithMessage("Los estilos de pelea no pueden estar vacios.");
            RuleFor(n => n.ClanId).NotEmpty().WithMessage("El Clan necesita un Id valido.");
            RuleFor(n => n.ReinoId).NotEmpty().WithMessage("El Reino necesita un Id valido.");
        }
    }
}
