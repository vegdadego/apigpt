using Application;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infrastructure
{
    public class EstudianteRepository : IEstudianteRepository
    {
        private readonly AppDbContext _context;
        public EstudianteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteMatriculaAsync(string matricula)
        {
            return await _context.Set<Estudiante>().AnyAsync(e => e.Matricula == matricula);
        }

        public async Task AgregarAsync(Estudiante estudiante)
        {
            await _context.Set<Estudiante>().AddAsync(estudiante);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TieneInscripcionesAsync(int estudianteId)
        {
            return await _context.Set<Inscripcion>().AnyAsync(i => i.EstudianteId == estudianteId);
        }

        public async Task EliminarAsync(int estudianteId)
        {
            var estudiante = await _context.Set<Estudiante>().FindAsync(estudianteId);
            if (estudiante != null)
            {
                _context.Set<Estudiante>().Remove(estudiante);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Estudiante>> GetAllAsync()
        {
            return await _context.Set<Estudiante>().ToListAsync();
        }

        public async Task<Estudiante?> GetByIdAsync(int id)
        {
            return await _context.Set<Estudiante>().FindAsync(id);
        }
    }
} 