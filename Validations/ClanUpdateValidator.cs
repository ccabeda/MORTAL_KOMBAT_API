﻿using FluentValidation;
using MortalKombat_API.Models.DTOs.ClanDTO;

namespace MiPrimeraAPI.Validations
{
    public class ClanUpdateValidator : AbstractValidator<ClanUpdateDto>
    {
        public ClanUpdateValidator()
        {
            RuleFor(i => i.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(n => n.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(n => n.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");

        }
    }
}