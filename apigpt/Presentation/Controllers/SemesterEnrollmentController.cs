using Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterEnrollmentController : ControllerBase
    {
        private readonly SemesterEnrollmentService _service;
        public SemesterEnrollmentController(SemesterEnrollmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Iniciar([FromBody] IniciarInscripcionDto dto)
        {
            var inscripcion = await _service.IniciarInscripcionAsync(dto.EstudianteId, dto.Semestre, dto.MaxCreditos);
            return Ok(inscripcion);
        }

        [HttpPost("{inscripcionId}/agregar-curso")]
        public async Task<IActionResult> AgregarCurso(int inscripcionId, [FromBody] AgregarCursoDto dto)
        {
            await _service.AgregarCursoAsync(inscripcionId, dto.CursoId);
            return Ok();
        }
    }
} 