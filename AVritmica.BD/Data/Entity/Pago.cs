using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(FechaPago), Name = "Pago_UQ", IsUnique = true)]
    public class Pago : EntityBase
    {
        // FK
        [Required(ErrorMessage = "El carrito es obligatoria")]
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        [Required(ErrorMessage = "La fecha de pago es obligatorio")]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo {1} caracteres.")]
        public string MetodoPago { get; set; }

        [Required(ErrorMessage = "El monto pagado es obligatorio")]
        public decimal MontoPagado { get; set; }

        [Required(ErrorMessage = "El estado del pago es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo {1} caracteres.")]
        public string EstadoPago { get; set; }

        [Required(ErrorMessage = "El saldo es obligatorio")]    
        public decimal Saldo { get; set; }
    }
}
