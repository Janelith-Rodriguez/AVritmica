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
    [Index(nameof(CarritoId), Name = "IX_Pagos_IdCarrito")]
    [Index(nameof(FechaPago), Name = "IX_Pagos_FechaPago")]
    [Index(nameof(EstadoPago), Name = "IX_Pagos_EstadoPago")]
    public class Pago : EntityBase
    {
        // Clave foránea
        public int CarritoId { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string MetodoPago { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoPagado { get; set; }

        [Required]
        [MaxLength(20)]
        public string EstadoPago { get; set; } = "Completado";

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        // Propiedad de navegación
        public virtual Carrito Carrito { get; set; }
    }
}
