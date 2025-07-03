namespace Entities
{
    // Entidad de dominio que representa un curso
    public class Curso : BaseEntity
    {
        public string Nombre { get; private set; }
        public int Creditos { get; private set; }
        public int CuposDisponibles { get; private set; }

        protected Curso() { }
        public Curso(string nombre, int creditos, int cuposDisponibles)
        {
            Nombre = nombre;
            Creditos = creditos;
            CuposDisponibles = cuposDisponibles;
        }
    }
} 