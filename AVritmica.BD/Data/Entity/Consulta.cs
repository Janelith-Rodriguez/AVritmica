using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(UsuarioId), Name = "IX_Consultas_IdUsuario")]
    [Index(nameof(Email), Name = "IX_Consultas_Email")]
    [Index(nameof(FechaEnvio), Name = "IX_Consultas_FechaEnvio")]
    public class Consulta : EntityBase
    {
        // Clave foránea
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Mensaje { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;

        // Propiedad de navegación
        public virtual Usuario Usuario { get; set; }
    }
}
