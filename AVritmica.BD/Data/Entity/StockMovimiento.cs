﻿using Microsoft.EntityFrameworkCore;
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
    public class StockMovimiento : EntityBase
    {
        // Clave foránea
        public int ProductoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoMovimiento { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public string Descripcion { get; set; } = string.Empty;

        // Propiedad de navegación
        public virtual Producto Producto { get; set; }
    }
}
