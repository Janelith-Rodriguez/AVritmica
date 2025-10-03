using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class CrearCompraDetalleDTO
    {

        // Claves foráneas
        //public int CompraId { get; set; }
        //public Compra Compra { get; set; }

        //public int ProductoId { get; set; }
        //public Producto Producto { get; set; }

        public int Cantidad { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioCompra { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioVentaActualizado { get; set; }
    }
}
