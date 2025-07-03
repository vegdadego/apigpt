using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Usuario : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
    }
} 