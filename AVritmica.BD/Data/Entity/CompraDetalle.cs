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
    [Index(nameof(CompraId), Name = "IX_CompraDetalle_IdCompra")]
    [Index(nameof(ProductoId), Name = "IX_CompraDetalle_IdProducto")]
    public class CompraDetalle : EntityBase
    {
        // Claves foráneas
        public int CompraId { get; set; }
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioCompra { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioVentaActualizado { get; set; }

        // Propiedades de navegación
        public virtual Compra Compra { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
