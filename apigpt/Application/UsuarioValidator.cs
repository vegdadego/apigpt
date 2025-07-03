using FluentValidation;

namespace Application
{
    public class UsuarioValidator : BaseValidator<UsuarioDto>
    {
        public UsuarioValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 letras.")
                .MaximumLength(50).WithMessage("El nombre no puede tener más de 50 caracteres.")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("El nombre solo puede contener letras y espacios.");

            RuleFor(x => x.Contraseña)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(4).WithMessage("La contraseña debe tener al menos 4 caracteres.");
        }
    }
} 