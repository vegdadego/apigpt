using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    // Agregado raíz que representa la inscripción de un semestre
    public class SemesterEnrollment : BaseEntity
    {
        public int EstudianteId { get; private set; }
        public string Semestre { get; private set; }
        public int MaxCreditHours { get; private set; }
        private readonly List<EnrolledCourse> _cursosInscritos = new();
        public IReadOnlyCollection<EnrolledCourse> CursosInscritos => _cursosInscritos.AsReadOnly();

        protected SemesterEnrollment() { }
        public SemesterEnrollment(int estudianteId, string semestre, int maxCreditHours)
        {
            EstudianteId = estudianteId;
            Semestre = semestre;
            MaxCreditHours = maxCreditHours;
        }

        // Regla de dominio: no exceder el límite de créditos
        public void AgregarCurso(EnrolledCourse curso)
        {
            int sumaActual = _cursosInscritos.Sum(c => c.CreditHours);
            if (sumaActual + curso.CreditHours > MaxCreditHours)
                throw new InvalidOperationException($"No se pueden inscribir más de {MaxCreditHours} créditos en el semestre.");
            _cursosInscritos.Add(curso);
        }
    }
} 