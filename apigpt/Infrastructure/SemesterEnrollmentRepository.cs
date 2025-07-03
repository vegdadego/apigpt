using Application;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SemesterEnrollmentRepository : ISemesterEnrollmentRepository
    {
        private readonly AppDbContext _context;
        public SemesterEnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(SemesterEnrollment inscripcion)
        {
            await _context.Set<SemesterEnrollment>().AddAsync(inscripcion);
            await _context.SaveChangesAsync();
        }

        public async Task<SemesterEnrollment> ObtenerPorIdAsync(int id)
        {
            return await _context.Set<SemesterEnrollment>().FindAsync(id);
        }

        public async Task ActualizarAsync(SemesterEnrollment inscripcion)
        {
            _context.Set<SemesterEnrollment>().Update(inscripcion);
            await _context.SaveChangesAsync();
        }
    }
} 