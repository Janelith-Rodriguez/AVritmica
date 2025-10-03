using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(FechaCompra), Name = "IX_Compras_FechaCompra")]
    public class Compra : EntityBase
    {
        public DateTime FechaCompra { get; set; } = DateTime.UtcNow;

        public string Descripcion { get; set; } = string.Empty;

        // Propiedad de navegación
        public List<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();
    }
}
