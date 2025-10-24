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
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Mensaje { get; set; } = string.Empty;

        public bool? Leida { get; set; } = false;

        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        // ESTO DEBE SER NULLABLE
        public int? UsuarioId { get; set; }  // ← Cambiar a nullable

        public virtual Usuario? Usuario { get; set; }  // ← También nullable
    }
}
