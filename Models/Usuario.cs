using System.ComponentModel.DataAnnotations;

namespace ProyectoInmobiliariaADO.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } // se guarda el hash, no la contraseña en texto plano

        [StringLength(255)]
        public string? Avatar { get; set; } // ruta de la imagen de perfil, puede ser null si no tiene avatar

        [Required]
        public RolUsuario Rol { get; set; } // Enum: Administrador o Empleado

        public bool Activo { get; set; } = true; // Baja lógica en lugar de borrar

        // Auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaUltimoLogin { get; set; }
    }

    public enum RolUsuario
    {
        Administrador = 1,
        Empleado = 2
    }
}
