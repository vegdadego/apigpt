using Entities;
using System;
using System.Threading.Tasks;

namespace Application
{
    // Servicio de aplicación para lógica de inscripción de semestre
    public class SemesterEnrollmentService
    {
        private readonly ISemesterEnrollmentRepository _repo;
        private readonly ICursoRepository _cursoRepo;
        public SemesterEnrollmentService(ISemesterEnrollmentRepository repo, ICursoRepository cursoRepo)
        {
            _repo = repo;
            _cursoRepo = cursoRepo;
        }

        public async Task<SemesterEnrollment> IniciarInscripcionAsync(int estudianteId, string semestre, int maxCreditos)
        {
            var inscripcion = new SemesterEnrollment(estudianteId, semestre, maxCreditos);
            await _repo.AgregarAsync(inscripcion);
            return inscripcion;
        }

        // Regla de dominio: no exceder el límite de créditos
        public async Task AgregarCursoAsync(int inscripcionId, int cursoId)
        {
            var inscripcion = await _repo.ObtenerPorIdAsync(inscripcionId);
            var curso = await _cursoRepo.ObtenerPorIdAsync(cursoId);
            var enrolled = new EnrolledCourse(0, curso.Id, curso.Nombre, curso.Creditos);
            inscripcion.AgregarCurso(enrolled); // Lógica de dominio: lanza excepción si se exceden créditos
            await _repo.ActualizarAsync(inscripcion);
        }
    }

    // Interfaces de repositorio para inyección de dependencias
    public interface ISemesterEnrollmentRepository
    {
        Task AgregarAsync(SemesterEnrollment inscripcion);
        Task<SemesterEnrollment> ObtenerPorIdAsync(int id);
        Task ActualizarAsync(SemesterEnrollment inscripcion);
    }
    public interface ICursoRepository
    {
        Task<Curso> ObtenerPorIdAsync(int id);
    }
} 