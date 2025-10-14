using AVritmica.BD.Data.Entity;
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
    [Index(nameof(Nombre), Name = "IX_Productos_Nombre")]
    public class Producto : EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public string ImagenUrl { get; set; } = string.Empty;

        // Clave foránea
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public List<CarritoProducto> CarritoProductos { get; set; } = new List<CarritoProducto>();
        public List<StockMovimiento> StockMovimientos { get; set; } = new List<StockMovimiento>();
        public List<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();
    }
}

