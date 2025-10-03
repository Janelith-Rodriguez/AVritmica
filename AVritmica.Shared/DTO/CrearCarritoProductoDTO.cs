using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class CrearCarritoProductoDTO
    {
        // Claves foráneas
        //public int CarritoId { get; set; }
        //public Carrito Carrito { get; set; }

        //public int ProductoId { get; set; }
        //public Producto Producto { get; set; }

        public int Cantidad { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }
    }
}
