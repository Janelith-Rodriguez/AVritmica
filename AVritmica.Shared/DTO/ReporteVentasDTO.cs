using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class ReporteVentasDTO
    {
        public DateTime Fecha { get; set; }
        public int CarritoId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string EstadoPago { get; set; } = string.Empty;
        public int CantidadProductos { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal Saldo { get; set; }
        public List<DetalleVentaDTO> Detalles { get; set; } = new List<DetalleVentaDTO>();
    }

    public class DetalleVentaDTO
    {
        public string Producto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class ResumenVentasDTO
    {
        public int TotalVentas { get; set; }
        public int VentasConfirmadas { get; set; }
        public int VentasPendientes { get; set; }
        public decimal TotalMontoVentas { get; set; }
        public decimal TotalMontoPagado { get; set; }
        public decimal TotalSaldoPendiente { get; set; }
        public int TotalProductosVendidos { get; set; }
        public List<VentasPorPeriodoDTO> VentasPorPeriodo { get; set; } = new List<VentasPorPeriodoDTO>();
    }

    public class VentasPorPeriodoDTO
    {
        public string Periodo { get; set; } = string.Empty;
        public int CantidadVentas { get; set; }
        public decimal TotalVentas { get; set; }
    }
}