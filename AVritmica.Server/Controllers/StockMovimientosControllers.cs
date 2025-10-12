using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/StockMovimientos")]
    public class StockMovimientosController : ControllerBase
    {
        private readonly IStockMovimientoRepositorio repositorio;

        public StockMovimientosController(IStockMovimientoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/StockMovimientos
        public async Task<ActionResult<List<StockMovimiento>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener un movimiento de stock por ID
        /// </summary>
        /// <param name="id">Id del movimiento</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] // api/StockMovimientos/2
        public async Task<ActionResult<StockMovimiento>> Get(int id)
        {
            StockMovimiento? movimiento = await repositorio.SelectById(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            return movimiento;
        }

        [HttpGet("GetByProducto/{productoId:int}")] // api/StockMovimientos/GetByProducto/1
        public async Task<ActionResult<List<StockMovimiento>>> GetByProducto(int productoId)
        {
            var movimientos = await repositorio.SelectByProducto(productoId);
            return movimientos;
        }

        [HttpGet("GetByTipoMovimiento/{tipoMovimiento}")] // api/StockMovimientos/GetByTipoMovimiento/Entrada
        public async Task<ActionResult<List<StockMovimiento>>> GetByTipoMovimiento(string tipoMovimiento)
        {
            var movimientos = await repositorio.SelectByTipoMovimiento(tipoMovimiento);
            return movimientos;
        }

        [HttpGet("GetByRangoFechas")] // api/StockMovimientos/GetByRangoFechas?fechaInicio=2024-01-01&fechaFin=2024-01-31
        public async Task<ActionResult<List<StockMovimiento>>> GetByRangoFechas([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            // Validar que la fecha de inicio no sea mayor que la fecha fin
            if (fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");
            }

            var movimientos = await repositorio.SelectByRangoFechas(fechaInicio, fechaFin);
            return movimientos;
        }

        [HttpGet("GetByProductoAndRangoFechas")] // api/StockMovimientos/GetByProductoAndRangoFechas?productoId=1&fechaInicio=2024-01-01&fechaFin=2024-01-31
        public async Task<ActionResult<List<StockMovimiento>>> GetByProductoAndRangoFechas([FromQuery] int productoId, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            // Validar que la fecha de inicio no sea mayor que la fecha fin
            if (fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");
            }

            var movimientos = await repositorio.SelectByProductoAndRangoFechas(productoId, fechaInicio, fechaFin);
            return movimientos;
        }

        [HttpGet("existe/{id:int}")] // api/StockMovimientos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("stock-actual/{productoId:int}")] // api/StockMovimientos/stock-actual/1
        public async Task<ActionResult<int>> ObtenerStockActual(int productoId)
        {
            var stock = await repositorio.ObtenerStockActual(productoId);
            return stock;
        }

        [HttpGet("entradas-totales/{productoId:int}")] // api/StockMovimientos/entradas-totales/1
        public async Task<ActionResult<int>> ObtenerEntradasTotales(int productoId)
        {
            var entradas = await repositorio.ObtenerEntradasTotales(productoId);
            return entradas;
        }

        [HttpGet("salidas-totales/{productoId:int}")] // api/StockMovimientos/salidas-totales/1
        public async Task<ActionResult<int>> ObtenerSalidasTotales(int productoId)
        {
            var salidas = await repositorio.ObtenerSalidasTotales(productoId);
            return salidas;
        }

        [HttpPost("registrar-movimiento")] // api/StockMovimientos/registrar-movimiento
        public async Task<ActionResult<bool>> RegistrarMovimiento([FromBody] RegistrarMovimientoRequest request)
        {
            try
            {
                // Validar que la cantidad sea diferente de cero
                if (request.Cantidad == 0)
                {
                    return BadRequest("La cantidad no puede ser cero");
                }

                // Validar tipos de movimiento
                var tiposValidos = new[] { "Entrada", "Salida", "Compra", "Venta", "Ajuste Positivo", "Ajuste Negativo" };
                if (!tiposValidos.Contains(request.TipoMovimiento))
                {
                    return BadRequest($"Tipo de movimiento no válido. Los tipos válidos son: {string.Join(", ", tiposValidos)}");
                }

                var resultado = await repositorio.RegistrarMovimiento(
                    request.ProductoId,
                    request.TipoMovimiento,
                    request.Cantidad,
                    request.Descripcion
                );

                if (!resultado)
                {
                    return BadRequest("No se pudo registrar el movimiento de stock");
                }

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("entrada-stock")] // api/StockMovimientos/entrada-stock
        public async Task<ActionResult<bool>> RegistrarEntradaStock([FromBody] RegistrarEntradaRequest request)
        {
            try
            {
                // Validar que la cantidad sea positiva
                if (request.Cantidad <= 0)
                {
                    return BadRequest("La cantidad debe ser mayor a cero para una entrada de stock");
                }

                var resultado = await repositorio.RegistrarMovimiento(
                    request.ProductoId,
                    "Entrada",
                    request.Cantidad,
                    request.Descripcion
                );

                if (!resultado)
                {
                    return BadRequest("No se pudo registrar la entrada de stock");
                }

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("salida-stock")] // api/StockMovimientos/salida-stock
        public async Task<ActionResult<bool>> RegistrarSalidaStock([FromBody] RegistrarSalidaRequest request)
        {
            try
            {
                // Validar que la cantidad sea positiva
                if (request.Cantidad <= 0)
                {
                    return BadRequest("La cantidad debe ser mayor a cero para una salida de stock");
                }

                // Verificar stock disponible
                var stockActual = await repositorio.ObtenerStockActual(request.ProductoId);
                if (stockActual < request.Cantidad)
                {
                    return BadRequest($"Stock insuficiente. Stock actual: {stockActual}, Cantidad solicitada: {request.Cantidad}");
                }

                var resultado = await repositorio.RegistrarMovimiento(
                    request.ProductoId,
                    "Salida",
                    request.Cantidad,
                    request.Descripcion
                );

                if (!resultado)
                {
                    return BadRequest("No se pudo registrar la salida de stock");
                }

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(StockMovimiento entidad)
        {
            try
            {
                // Validar que la cantidad sea diferente de cero
                if (entidad.Cantidad == 0)
                {
                    return BadRequest("La cantidad no puede ser cero");
                }

                // Validar tipos de movimiento
                var tiposValidos = new[] { "Entrada", "Salida", "Compra", "Venta", "Ajuste Positivo", "Ajuste Negativo" };
                if (!tiposValidos.Contains(entidad.TipoMovimiento))
                {
                    return BadRequest($"Tipo de movimiento no válido. Los tipos válidos son: {string.Join(", ", tiposValidos)}");
                }

                return await repositorio.Insert(entidad);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")] // api/StockMovimientos/2
        public async Task<ActionResult> Put(int id, [FromBody] StockMovimiento entidad)
        {
            try
            {
                if (id != entidad.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                // Validar que la cantidad sea diferente de cero
                if (entidad.Cantidad == 0)
                {
                    return BadRequest("La cantidad no puede ser cero");
                }

                // Validar tipos de movimiento
                var tiposValidos = new[] { "Entrada", "Salida", "Compra", "Venta", "Ajuste Positivo", "Ajuste Negativo" };
                if (!tiposValidos.Contains(entidad.TipoMovimiento))
                {
                    return BadRequest($"Tipo de movimiento no válido. Los tipos válidos son: {string.Join(", ", tiposValidos)}");
                }

                var resultado = await repositorio.Update(id, entidad);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el movimiento de stock");
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/StockMovimientos/2
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await repositorio.Delete(id);
            if (!resp)
            {
                return BadRequest("El movimiento de stock no se pudo borrar");
            }
            return Ok();
        }
    }

    // Clases auxiliares para requests
    public class RegistrarMovimientoRequest
    {
        public int ProductoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoMovimiento { get; set; } = string.Empty;

        [Required]
        public int Cantidad { get; set; }

        public string Descripcion { get; set; } = string.Empty;
    }

    public class RegistrarEntradaRequest
    {
        public int ProductoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

        public string Descripcion { get; set; } = string.Empty;
    }

    public class RegistrarSalidaRequest
    {
        public int ProductoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

        public string Descripcion { get; set; } = string.Empty;
    }
}
