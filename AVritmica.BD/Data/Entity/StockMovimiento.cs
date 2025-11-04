using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(ProductoId), Name = "IX_StockMovimientos_ProductoId")]
    [Index(nameof(TipoMovimiento), Name = "IX_StockMovimientos_TipoMovimiento")]
    [Index(nameof(CarritoId), Name = "IX_StockMovimientos_CarritoId")]
    [Index(nameof(CompraId), Name = "IX_StockMovimientos_CompraId")] // NUEVO
    [Index(nameof(Proveedor), Name = "IX_StockMovimientos_Proveedor")]
    public class StockMovimiento : EntityBase
    {
        // Clave foránea principal
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoMovimiento { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public string Descripcion { get; set; } = string.Empty;

        // RELACIÓN CON CARRITO (para salidas/ventas)
        public int? CarritoId { get; set; }
        public Carrito? Carrito { get; set; }

        // NUEVA RELACIÓN CON COMPRA (para entradas/compras)
        public int? CompraId { get; set; }
        public Compra? Compra { get; set; }

        // PROPIEDADES PARA PROVEEDORES
        [MaxLength(100)]
        public string? Proveedor { get; set; }

        [MaxLength(50)]
        public string? NumeroFactura { get; set; }

        [MaxLength(20)]
        public string? NumeroOrdenCompra { get; set; }

        // Para seguimiento interno
        [MaxLength(50)]
        public string? UsuarioRegistro { get; set; }

        public DateTime? FechaDocumento { get; set; }
    }
}
