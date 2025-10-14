using System.ComponentModel.DataAnnotations;

namespace AVritmica.Shared.DTO
{
    public class CrearProductoDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        public string ImagenUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es requerida")]
        public int CategoriaId { get; set; } // ¡Esto faltaba!
    }
}
