using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(UsuarioId), Name = "IX_Carritos_UsuarioId")]
    [Index(nameof(Estado), Name = "IX_Carritos_Estado")]
    [Index(nameof(FechaCreacion), Name = "IX_Carritos_FechaCreacion")]
    [Index(nameof(EstadoPago), Name = "IX_Carritos_EstadoPago")]
    public class Carrito : EntityBase
    {
        // Clave foránea
        public int UsuarioId { get; set; }

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

        // Propiedades de navegación
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<CarritoProducto> CarritoProductos { get; set; } = new List<CarritoProducto>();
        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
