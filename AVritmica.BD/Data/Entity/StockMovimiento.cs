using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(TipoMovimiento), Name = "StockMovimiento_UQ", IsUnique = true)]
    public class StockMovimiento : EntityBase
    {
        // FK
        [Required(ErrorMessage = "El producto es obligatoria")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio")]
        [MaxLength(50, ErrorMessage = "Máximo {1} caracteres.")]
        public string TipoMovimiento { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [MaxLength(500, ErrorMessage = "Máximo {1} caracteres.")]
        public string Descripcion { get; set; }
    }
}
