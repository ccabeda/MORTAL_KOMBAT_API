﻿using FluentValidation;
using MortalKombat_API.Models.DTOs.ReinoDTO;

namespace MiPrimeraAPI.Validations
{
    public class ReinoUpdateValidator : AbstractValidator<ReinoUpdateDto>
    {
        public ReinoUpdateValidator()
        {
            RuleFor(i => i.Id).NotEqual(0).WithMessage("El id no puede ser 0");
            RuleFor(n => n.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.");
            RuleFor(n => n.Descripcion).NotEmpty().WithMessage("La descripción no puede estar vacio.");
        }
    }
}