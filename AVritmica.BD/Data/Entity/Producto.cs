using AVritmica.BD.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(CategotiaId), nameof(Nombre), Name = "Producto_UQ", IsUnique = true)] // Índice único por Categoría + Nombre
    [Index(nameof(Precio), nameof(Descripcion), Name = "Producto_Precio", IsUnique = false)] // Índice en Precio
    public class Producto : EntityBase
    {
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [MaxLength(150, ErrorMessage = "Maximo numero de caracteres{1}.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción del producto es obligatorio")]
        [MaxLength(200, ErrorMessage = "Maximo numero de caracteres{1}.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio del producto es obligatori")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock del producto es obligatorio")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La imagen url del producto es obligatorio")]
        [MaxLength(250, ErrorMessage = "Maximo numero de caracteres{1}.")]
        public string Imagen_Url { get; set; }

        [Required(ErrorMessage = "La categoria es obligatoria")]
        public int CategotiaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}

