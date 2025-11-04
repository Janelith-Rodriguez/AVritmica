// Repositorio/IReportesRepositorio.cs
using AVritmica.Shared.DTO;

namespace AVritmica.Server.Repositorio
{
    public interface IReportesRepositorio
    {
        // Reportes de Ventas
        Task<List<ReporteVentasDTO>> ObtenerVentasPorPeriodo(DateTime fechaInicio, DateTime fechaFin);
        Task<ResumenVentasDTO> ObtenerResumenVentas(DateTime fechaInicio, DateTime fechaFin);
        Task<List<ReporteVentasDTO>> ObtenerVentasPorUsuario(int usuarioId);
        Task<List<ReporteVentasDTO>> ObtenerVentasPorEstado(string estado);

        // Reportes de Stock
        Task<List<ReporteStockDTO>> ObtenerReporteStock();
        Task<ResumenStockDTO> ObtenerResumenStock();
        Task<List<ReporteStockDTO>> ObtenerProductosStockBajo();
        Task<List<ReporteStockDTO>> ObtenerMovimientosStockPorProducto(int productoId, DateTime fechaInicio, DateTime fechaFin);

        // Reportes de Compras
        Task<List<ReporteComprasDTO>> ObtenerComprasPorPeriodo(DateTime fechaInicio, DateTime fechaFin);
        Task<ResumenComprasDTO> ObtenerResumenCompras(DateTime fechaInicio, DateTime fechaFin);

        // Reportes de Usuarios
        Task<List<ReporteUsuariosDTO>> ObtenerReporteUsuarios();
        Task<ResumenUsuariosDTO> ObtenerResumenUsuarios();
        Task<List<ReporteUsuariosDTO>> ObtenerUsuariosPorTipo(string tipoUsuario);
    }
}