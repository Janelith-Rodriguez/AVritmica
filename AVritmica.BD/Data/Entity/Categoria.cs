using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(Nombre), Name = "Categoria_UQ", IsUnique = true)] // Índice único en Nombre
    public class Categoria : EntityBase
    {
        [Required(ErrorMessage = "El nombre de la categoria es obligatorio")]
        [MaxLength(100, ErrorMessage = "Maximo numero de caracteres{1}.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción de la categoria es obligatorio")]
        [MaxLength(150, ErrorMessage = "Maximo numero de caracteres{1}.")]
        public string Descripcion { get; set; }
    }
}
