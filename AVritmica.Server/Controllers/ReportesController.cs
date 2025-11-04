using Microsoft.AspNetCore.Mvc;
using AVritmica.Server.Repositorio;
using AVritmica.Shared.DTO;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesRepositorio _reportesRepositorio;

        public ReportesController(IReportesRepositorio reportesRepositorio)
        {
            _reportesRepositorio = reportesRepositorio;
        }

        // VENTAS
        [HttpGet("ventas/periodo")]
        public async Task<ActionResult<List<ReporteVentasDTO>>> GetVentasPorPeriodo(
            [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");

                var ventas = await _reportesRepositorio.ObtenerVentasPorPeriodo(fechaInicio, fechaFin);
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener ventas: {ex.Message}");
            }
        }

        [HttpGet("ventas/resumen")]
        public async Task<ActionResult<ResumenVentasDTO>> GetResumenVentas(
            [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");

                var resumen = await _reportesRepositorio.ObtenerResumenVentas(fechaInicio, fechaFin);
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener resumen de ventas: {ex.Message}");
            }
        }

        [HttpGet("ventas/usuario/{usuarioId}")]
        public async Task<ActionResult<List<ReporteVentasDTO>>> GetVentasPorUsuario(int usuarioId)
        {
            try
            {
                var ventas = await _reportesRepositorio.ObtenerVentasPorUsuario(usuarioId);
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener ventas del usuario: {ex.Message}");
            }
        }

        [HttpGet("ventas/estado/{estado}")]
        public async Task<ActionResult<List<ReporteVentasDTO>>> GetVentasPorEstado(string estado)
        {
            try
            {
                var ventas = await _reportesRepositorio.ObtenerVentasPorEstado(estado);
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener ventas por estado: {ex.Message}");
            }
        }

        // STOCK
        [HttpGet("stock")]
        public async Task<ActionResult<List<ReporteStockDTO>>> GetReporteStock()
        {
            try
            {
                var stock = await _reportesRepositorio.ObtenerReporteStock();
                return Ok(stock);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener reporte de stock: {ex.Message}");
            }
        }

        [HttpGet("stock/resumen")]
        public async Task<ActionResult<ResumenStockDTO>> GetResumenStock()
        {
            try
            {
                var resumen = await _reportesRepositorio.ObtenerResumenStock();
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener resumen de stock: {ex.Message}");
            }
        }

        [HttpGet("stock/bajo")]
        public async Task<ActionResult<List<ReporteStockDTO>>> GetProductosStockBajo()
        {
            try
            {
                var productos = await _reportesRepositorio.ObtenerProductosStockBajo();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener productos con stock bajo: {ex.Message}");
            }
        }

        [HttpGet("stock/producto/{productoId}/movimientos")]
        public async Task<ActionResult<List<ReporteStockDTO>>> GetMovimientosStockPorProducto(
            int productoId, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");

                var movimientos = await _reportesRepositorio.ObtenerMovimientosStockPorProducto(productoId, fechaInicio, fechaFin);
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener movimientos de stock: {ex.Message}");
            }
        }

        // COMPRAS
        [HttpGet("compras/periodo")]
        public async Task<ActionResult<List<ReporteComprasDTO>>> GetComprasPorPeriodo(
            [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");

                var compras = await _reportesRepositorio.ObtenerComprasPorPeriodo(fechaInicio, fechaFin);
                return Ok(compras);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener compras: {ex.Message}");
            }
        }

        [HttpGet("compras/resumen")]
        public async Task<ActionResult<ResumenComprasDTO>> GetResumenCompras(
            [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");

                var resumen = await _reportesRepositorio.ObtenerResumenCompras(fechaInicio, fechaFin);
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener resumen de compras: {ex.Message}");
            }
        }

        // USUARIOS
        [HttpGet("usuarios")]
        public async Task<ActionResult<List<ReporteUsuariosDTO>>> GetReporteUsuarios()
        {
            try
            {
                var usuarios = await _reportesRepositorio.ObtenerReporteUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener reporte de usuarios: {ex.Message}");
            }
        }

        [HttpGet("usuarios/resumen")]
        public async Task<ActionResult<ResumenUsuariosDTO>> GetResumenUsuarios()
        {
            try
            {
                var resumen = await _reportesRepositorio.ObtenerResumenUsuarios();
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener resumen de usuarios: {ex.Message}");
            }
        }

        [HttpGet("usuarios/tipo/{tipoUsuario}")]
        public async Task<ActionResult<List<ReporteUsuariosDTO>>> GetUsuariosPorTipo(string tipoUsuario)
        {
            try
            {
                var usuarios = await _reportesRepositorio.ObtenerUsuariosPorTipo(tipoUsuario);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener usuarios por tipo: {ex.Message}");
            }
        }
    }
}
