using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class ReporteComprasDTO
    {
        public int CompraId { get; set; }
        public DateTime FechaCompra { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int CantidadProductos { get; set; }
        public int TotalUnidades { get; set; }
        public decimal TotalCompra { get; set; }
        public List<DetalleCompraDTO> Detalles { get; set; } = new List<DetalleCompraDTO>();
    }

    public class DetalleCompraDTO
    {
        public string Producto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class ResumenComprasDTO
    {
        public int TotalCompras { get; set; }
        public int TotalProductosComprados { get; set; }
        public int TotalUnidadesCompradas { get; set; }
        public decimal TotalInvertido { get; set; }
        public decimal PromedioCompra { get; set; }
        public List<ComprasPorPeriodoDTO> ComprasPorPeriodo { get; set; } = new List<ComprasPorPeriodoDTO>();
    }

    public class ComprasPorPeriodoDTO
    {
        public string Periodo { get; set; } = string.Empty;
        public int CantidadCompras { get; set; }
        public decimal TotalCompras { get; set; }
    }
}
