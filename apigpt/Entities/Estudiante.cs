using System;
using System.Collections.Generic;

namespace Entities
{
    // Entidad de dominio que representa a un estudiante
    public class Estudiante : BaseEntity
    {
        public string Nombre { get; private set; }
        public string Matricula { get; private set; }
        public string Carrera { get; private set; }
        public int SemestreActual { get; private set; }
        public int CreditosMaximosPorSemestre { get; private set; }

        // Constructor protegido para EF/Core
        protected Estudiante() { }

        public Estudiante(string nombre, string matricula, string carrera, int semestreActual, int creditosMaximosPorSemestre)
        {
            if (string.IsNullOrWhiteSpace(matricula))
                throw new ArgumentException("La matrícula es obligatoria");
            // La verificación de unicidad real se hace en infraestructura, pero la lógica de negocio y la excepción deben estar aquí
            Nombre = nombre;
            Matricula = matricula;
            Carrera = carrera;
            SemestreActual = semestreActual;
            CreditosMaximosPorSemestre = creditosMaximosPorSemestre;
        }
    }
} 