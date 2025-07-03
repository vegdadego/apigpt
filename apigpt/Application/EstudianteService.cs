using Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application
{
    // Servicio de aplicación para lógica de estudiantes
    public class EstudianteService
    {
        private readonly IEstudianteRepository _repo;
        public EstudianteService(IEstudianteRepository repo)
        {
            _repo = repo;
        }

        // Regla de negocio: no permitir duplicación de matrícula
        public async Task<Estudiante> CrearEstudianteAsync(string nombre, string matricula, string carrera, int semestreActual, int creditosMaximosPorSemestre)
        {
            if (await _repo.ExisteMatriculaAsync(matricula))
                throw new InvalidOperationException("Ya existe un estudiante con esa matrícula.");
            var estudiante = new Estudiante(nombre, matricula, carrera, semestreActual, creditosMaximosPorSemestre);
            await _repo.AgregarAsync(estudiante);
            return estudiante;
        }

        // Regla de negocio: no permitir eliminar estudiantes con inscripciones
        public async Task EliminarEstudianteAsync(int estudianteId)
        {
            if (await _repo.TieneInscripcionesAsync(estudianteId))
                throw new InvalidOperationException("No se puede eliminar un estudiante con inscripciones activas.");
            await _repo.EliminarAsync(estudianteId);
        }

        // Obtener todos los estudiantes
        public async Task<IEnumerable<Estudiante>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        // Obtener estudiante por id
        public async Task<Estudiante?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        // Actualizar estudiante
        public async Task<bool> ActualizarAsync(int id, object dto)
        {
            var estudiante = await _repo.GetByIdAsync(id);
            if (estudiante == null)
                return false;
            var datos = dto as dynamic;
            estudiante.GetType().GetProperty("Nombre")?.SetValue(estudiante, datos.Nombre);
            estudiante.GetType().GetProperty("Carrera")?.SetValue(estudiante, datos.Carrera);
            estudiante.GetType().GetProperty("SemestreActual")?.SetValue(estudiante, datos.SemestreActual);
            estudiante.GetType().GetProperty("CreditosMaximosPorSemestre")?.SetValue(estudiante, datos.CreditosMaximosPorSemestre);
            await _repo.AgregarAsync(estudiante); // O usa un método UpdateAsync si lo tienes
            return true;
        }
    }

    // Interfaz de repositorio para inyección de dependencias
    public interface IEstudianteRepository
    {
        Task<bool> ExisteMatriculaAsync(string matricula);
        Task AgregarAsync(Estudiante estudiante);
        Task<bool> TieneInscripcionesAsync(int estudianteId);
        Task EliminarAsync(int estudianteId);
        Task<IEnumerable<Estudiante>> GetAllAsync();
        Task<Estudiante?> GetByIdAsync(int id);
    }
} 