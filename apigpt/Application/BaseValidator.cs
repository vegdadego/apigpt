using FluentValidation;

namespace Application
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        public BaseValidator()
        {
            // Aquí puedes poner reglas comunes para todos los DTOs si lo deseas
        }
    }
} 