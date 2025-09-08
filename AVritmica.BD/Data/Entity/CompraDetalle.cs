using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(CompraId), nameof(ProductoId), Name = "CompraDetalle_UQ", IsUnique = true)]
    public class CompraDetalle : EntityBase
    {
        // FK con Compra
        [Required(ErrorMessage = "La compra es obligatoria")]
        public int CompraId { get; set; }
        public Compra Compra { get; set; }

        // FK con Producto
        [Required(ErrorMessage = "El producto es obligatorio")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio de compra es obligatorio")]
        public decimal PrecioCompra { get; set; }

        [Required(ErrorMessage = "El precio de venta actualizado es obligatorio")]
        public decimal Precio_Venta_Actualizado { get; set; }
    }
}
