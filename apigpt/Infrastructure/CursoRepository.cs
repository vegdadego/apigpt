using Application;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class CursoRepository : ICursoRepository
    {
        private readonly AppDbContext _context;
        public CursoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Curso> ObtenerPorIdAsync(int id)
        {
            return await _context.Set<Curso>().FindAsync(id);
        }
    }
} 