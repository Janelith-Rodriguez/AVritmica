using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class CompraDetalleDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La compra es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La compra es requerida")]
        public int CompraId { get; set; }

        [Required(ErrorMessage = "El producto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El producto es requerido")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio de compra es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor a 0")]
        public decimal PrecioCompra { get; set; }

        [Required(ErrorMessage = "El precio de venta es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor a 0")]
        public decimal PrecioVentaActualizado { get; set; }
    }

    public class ActualizarPreciosRequest
    {
        [Required(ErrorMessage = "El precio de compra es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor a 0")]
        public decimal PrecioCompra { get; set; }

        [Required(ErrorMessage = "El precio de venta es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor a 0")]
        public decimal PrecioVentaActualizado { get; set; }
    }

    public class ValidacionResponse
    {
        public bool Existe { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}