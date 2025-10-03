using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class CrearPagoDTO
    {
        // Clave foránea
        //public int CarritoId { get; set; }
        //public Carrito Carrito { get; set; }

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
    }
}
