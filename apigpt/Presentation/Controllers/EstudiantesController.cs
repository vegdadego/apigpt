using Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly EstudianteService _service;
        public EstudiantesController(EstudianteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEstudianteDto dto)
        {
            var estudiante = await _service.CrearEstudianteAsync(dto.Nombre, dto.Matricula, dto.Carrera, dto.SemestreActual, dto.CreditosMaximosPorSemestre);
            return Ok(estudiante);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");
            await _service.EliminarEstudianteAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            if (page <= 0)
                return BadRequest("El número de página debe ser mayor a 0.");
            if (pageSize <= 0 || pageSize > 50)
                return BadRequest("El tamaño de página debe ser entre 1 y 50.");
            var estudiantes = await _service.GetAllAsync(page, pageSize);
            return Ok(estudiantes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");
            var estudiante = await _service.GetByIdAsync(id);
            if (estudiante == null)
                return NotFound();
            return Ok(estudiante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarEstudianteDto dto)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");
            var actualizado = await _service.ActualizarAsync(id, dto);
            if (!actualizado)
                return NotFound();
            return NoContent();
        }
    }
} 