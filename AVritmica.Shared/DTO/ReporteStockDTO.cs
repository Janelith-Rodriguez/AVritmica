using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class ReporteStockDTO
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; } = 10; // Puedes ajustar este valor
        public int EntradasTotales { get; set; }
        public int SalidasTotales { get; set; }
        public decimal Precio { get; set; }
        public decimal ValorStock { get; set; }
        public string EstadoStock { get; set; } = "Normal";
        public List<MovimientoStockDTO> UltimosMovimientos { get; set; } = new List<MovimientoStockDTO>();
    }

    public class MovimientoStockDTO
    {
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class ResumenStockDTO
    {
        public int TotalProductos { get; set; }
        public int ProductosConStockBajo { get; set; }
        public int ProductosSinStock { get; set; }
        public int TotalStock { get; set; }
        public decimal ValorTotalStock { get; set; }
        public int MovimientosHoy { get; set; }
        public List<ProductosStockBajoDTO> ProductosStockBajo { get; set; } = new List<ProductosStockBajoDTO>();
    }

    public class ProductosStockBajoDTO
    {
        public string Producto { get; set; } = string.Empty;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}
