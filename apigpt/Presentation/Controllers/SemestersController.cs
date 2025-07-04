using Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api")]
public class SemestersController : ControllerBase
{
    private readonly SemesterEnrollmentService _service;
    private readonly EstudianteService _estudianteService;

    public SemestersController(SemesterEnrollmentService service, EstudianteService estudianteService)
    {
        _service = service;
        _estudianteService = estudianteService;
    }

    // GET /api/students/{studentId}/semesters
    [HttpGet("students/{studentId}/semesters")]
    public async Task<IActionResult> GetSemestersByStudent(int studentId)
    {
        var estudiante = await _estudianteService.GetByIdAsync(studentId);
        if (estudiante == null)
            return NotFound();
        var semestres = await _service.GetSemestersByStudentAsync(studentId);
        return Ok(semestres);
    }

    // PUT /api/students/{studentId}/semesters/{semestre}/courses/{courseId}
    [HttpPut("students/{studentId}/semesters/{semestre}/courses/{courseId}")]
    public async Task<IActionResult> EditCourseInSemester(int studentId, string semestre, int courseId, [FromBody] EditEnrolledCourseDto dto)
    {
        var result = await _service.EditEnrolledCourseAsync(studentId, semestre, courseId, dto.Nombre, dto.CreditHours);
        if (result == EditCourseResult.SemesterNotFound)
            return NotFound("Semestre no encontrado");
        if (result == EditCourseResult.CourseNotFound)
            return NotFound("Curso no encontrado en el semestre");
        if (result == EditCourseResult.CreditLimitExceeded)
            return BadRequest("El curso excede el máximo de créditos permitidos");
        return NoContent();
    }

    // DELETE /api/students/{studentId}/semesters/{semestre}/courses/{courseId}
    [HttpDelete("students/{studentId}/semesters/{semestre}/courses/{courseId}")]
    public async Task<IActionResult> DeleteCourseFromSemester(int studentId, string semestre, int courseId)
    {
        var result = await _service.DeleteEnrolledCourseAsync(studentId, semestre, courseId);
        if (result == DeleteCourseResult.SemesterNotFound)
            return NotFound("Semestre no encontrado");
        if (result == DeleteCourseResult.CourseNotFound)
            return NotFound("Curso no encontrado en el semestre");
        return NoContent();
    }
}

public class EditEnrolledCourseDto
{
    public required string Nombre { get; set; }
    public int CreditHours { get; set; }
} 