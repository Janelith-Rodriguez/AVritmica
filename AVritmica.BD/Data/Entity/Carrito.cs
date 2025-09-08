using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(FechaCreacion), Name = "Carrito_UQ", IsUnique = true)]
    public class Carrito : EntityBase
    {
        //FK
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required(ErrorMessage = "La fecha  de creación es obligatoria")]
        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "El estado el carrito es obligatorio")]
        [MaxLength(50, ErrorMessage = "Máximo {1} caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "La fecha  de confirmación es obligatoria")]
        public DateTime FechaConfirmacion { get; set; }

        [Required(ErrorMessage = "El estado de pago es obligatorio")]
        [MaxLength(50, ErrorMessage = "Máximo {1} caracteres.")]
        public string EstadoPago { get; set; }

        [Required(ErrorMessage = "El monto total es obligatorio")]
        public decimal MontoTotal { get; set; }

        [Required(ErrorMessage = "El saldo es obligatorio")]
        public decimal Saldo { get; set; }

        [Required(ErrorMessage = "La dirección de envio es obligatorio")]
        [MaxLength(50, ErrorMessage = "Máximo {1} caracteres.")]
        public string DireccionEnvio { get; set; }

        // Relaciones
        public List<CarritoProducto> CarritoProductos { get; set; }
        public List<Pago> Pagos { get; set; }
    }
}
