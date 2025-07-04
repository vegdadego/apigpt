using Infrastructure;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de DbContext con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Inyección de dependencias para el repositorio genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registro de servicios de aplicación
builder.Services.AddScoped<Application.EstudianteService>();
builder.Services.AddScoped<Application.SemesterEnrollmentService>();

// Registro de repositorios personalizados
builder.Services.AddScoped<Application.IEstudianteRepository, Infrastructure.EstudianteRepository>();
builder.Services.AddScoped<Application.ISemesterEnrollmentRepository, Infrastructure.SemesterEnrollmentRepository>();
builder.Services.AddScoped<Application.ICursoRepository, Infrastructure.CursoRepository>();

// 3. Configuración de CORS (permitir cualquier origen, método y header)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// 4. Configuración de autenticación JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

// 5. Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Examen API", Version = "v1" });
    // Configuración para JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Authorization: Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<Application.UsuarioValidator>();

builder.Services.AddControllers();

var app = builder.Build();

// Usar CORS
app.UseCors("AllowAll");

// Usar Swagger en desarrollo y producción
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Examen API v1");
    c.RoutePrefix = "swagger";
});

// Usar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Middleware global para logging de errores (diagnóstico temporal)
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Loguear el error en consola y en el response
        Console.WriteLine($"[ERROR GLOBAL] {ex.Message}\n{ex.StackTrace}");
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync($"{{\"error\":\"{ex.Message}\",\"stack\":\"{ex.StackTrace?.Replace("\n", " ").Replace("\"", "'") }\"}}");
    }
});

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
