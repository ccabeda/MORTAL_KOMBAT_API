using API_MortalKombat.Models.DTOs.UsuarioDTO;
using FluentValidation;

namespace API_MortalKombat.Validations
{
    public class UsuarioCreateValidator : AbstractValidator<UsuarioCreateDto>
    {
        public UsuarioCreateValidator()
        {
            RuleFor(u => u.NombreDeUsuario).NotEmpty().WithMessage("El nombre de usuario no puede estar vacio.");
            RuleFor(u => u.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(u => u.Apellido).NotEmpty().WithMessage("El apellido no puede estar vacio.");
            RuleFor(u => u.Mail).EmailAddress().WithMessage("El Mail no es valido.");
            RuleFor(u => u.Contraseña).NotEmpty().WithMessage("La contraseña no puede estar vacio.").MinimumLength(6).WithMessage("La contraseña es muy corto.");
        }
    }
}
