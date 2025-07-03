using Application;
using Core;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GenericController<T, TDto> : ControllerBase where T : BaseEntity where TDto : BaseDto
    {
        private readonly IRepository<T> _repository;
        private readonly IValidator<TDto> _validator;

        public GenericController(IRepository<T> repository, IValidator<TDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            if (page <= 0)
                return BadRequest("La página debe ser 1 o mayor. El parámetro 'page' no puede ser menor o igual a cero.");
            if (pageSize < 1)
                return BadRequest("El tamaño de página debe ser al menos 1. El parámetro 'pageSize' no puede ser menor a uno.");
            if (pageSize > 50)
                return BadRequest("El tamaño máximo por página es 50. El parámetro 'pageSize' no puede ser mayor a 50.");
            var result = await _repository.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TDto dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);
            // Aquí deberías mapear el DTO a la entidad T
            return StatusCode(501, "Implementa el mapeo de DTO a entidad y la lógica de guardado.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TDto dto)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);
            // Aquí deberías mapear el DTO a la entidad T y actualizar
            return StatusCode(501, "Implementa el mapeo de DTO a entidad y la lógica de actualización.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
} 