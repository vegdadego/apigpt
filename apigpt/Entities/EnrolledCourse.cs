namespace Entities
{
    // Valor-objeto que representa un curso inscrito en un semestre
    public class EnrolledCourse
    {
        public int Id { get; private set; }
        public int CursoId { get; private set; }
        public string Nombre { get; private set; }
        public int CreditHours { get; private set; }

        public EnrolledCourse(int id, int cursoId, string nombre, int creditHours)
        {
            Id = id;
            CursoId = cursoId;
            Nombre = nombre;
            CreditHours = creditHours;
        }
    }
} 