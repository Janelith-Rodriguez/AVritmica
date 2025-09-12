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
    [Index(nameof(CategoriaId), Name = "IX_Productos_IdCategoria")]
    [Index(nameof(Precio), Name = "IX_Productos_Precio")]
    [Index(nameof(Activo), Name = "IX_Productos_Activo")]
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

        public bool Activo { get; set; } = true;
        // Clave foránea
        public int CategoriaId { get; set; }

        // Propiedad de navegación
        public virtual Categoria Categoria { get; set; }
        public virtual ICollection<CarritoProducto> CarritoProductos { get; set; } = new List<CarritoProducto>();
        public virtual ICollection<StockMovimiento> StockMovimientos { get; set; } = new List<StockMovimiento>();
        public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();
    }
}

