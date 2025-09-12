using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(Email), IsUnique = true, Name = "IX_Usuarios_Email")]
    [Index(nameof(TipoUsuario), Name = "IX_Usuarios_TipoUsuario")]
    public class Usuario : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Contrasena { get; set; } = string.Empty;

        [Phone]
        public string Telefono { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        [Required]
        public string TipoUsuario { get; set; } = "Cliente";

        // Propiedades de navegación
        public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
        public virtual ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();

    }
}
