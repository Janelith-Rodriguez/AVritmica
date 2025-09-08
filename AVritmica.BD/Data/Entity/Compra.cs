using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(FechaCompra), Name = "Compra_UQ", IsUnique = true)]
    public class Compra : EntityBase
    {
        [Required(ErrorMessage = "La fecha de la compra es obligatoria")]
        public DateTime FechaCompra { get; set; }

        [MaxLength(200, ErrorMessage = "La descripción tiene como máximo{1} caracteres.")]
        public string Descripcion { get; set; }

        // Relación con CompraDetalle
        public List<CompraDetalle> CompraDetalles { get; set; }
    }
}
