using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(Nombre), IsUnique = true, Name = "IX_Categorias_Nombre")]
    public class Categoria : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        // Propiedad de navegación
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
