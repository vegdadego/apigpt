namespace Entities
{
    // Entidad de dominio que representa la inscripción de un estudiante a un curso
    public class Inscripcion : BaseEntity
    {
        public int EstudianteId { get; private set; }
        public int CursoId { get; private set; }
        public string Semestre { get; private set; }

        protected Inscripcion() { }
        public Inscripcion(int estudianteId, int cursoId, string semestre)
        {
            EstudianteId = estudianteId;
            CursoId = cursoId;
            Semestre = semestre;
        }
    }
} 