using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class CrearStockMovimientoDTO
    {
        // Clave foránea
        //public int ProductoId { get; set; }
        //public Producto Producto { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoMovimiento { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public string Descripcion { get; set; } = string.Empty;
    }
}
