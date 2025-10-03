using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class CrearCarritoDTO
    {
        // Clave foránea
        //public int UsuarioId { get; set; }
        //public Usuario Usuario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string Estado { get; set; } = "Activo";

        public DateTime? FechaConfirmacion { get; set; }

        [Required]
        [MaxLength(20)]
        public string EstadoPago { get; set; } = "Pendiente";

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoTotal { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        public string DireccionEnvio { get; set; } = string.Empty;
    }
}
