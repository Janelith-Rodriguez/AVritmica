using Microsoft.AspNetCore.Mvc;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/CompraDetalles")]
    public class CompraDetallesController : ControllerBase
    {
        private readonly ICompraDetalleRepositorio repositorio;

        public CompraDetallesController(ICompraDetalleRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/CompraDetalles
        public async Task<ActionResult<List<CompraDetalle>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener un detalle de compra por ID
        /// </summary>
        /// <param name="id">Id del detalle de compra</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] // api/CompraDetalles/2
        public async Task<ActionResult<CompraDetalle>> Get(int id)
        {
            CompraDetalle? compraDetalle = await repositorio.SelectById(id);
            if (compraDetalle == null)
            {
                return NotFound();
            }
            return compraDetalle;
        }

        [HttpGet("GetByCompra/{compraId:int}")] // api/CompraDetalles/GetByCompra/1
        public async Task<ActionResult<List<CompraDetalle>>> GetByCompra(int compraId)
        {
            var compraDetalles = await repositorio.SelectByCompra(compraId);
            return compraDetalles;
        }

        [HttpGet("GetByProducto/{productoId:int}")] // api/CompraDetalles/GetByProducto/1
        public async Task<ActionResult<List<CompraDetalle>>> GetByProducto(int productoId)
        {
            var compraDetalles = await repositorio.SelectByProducto(productoId);
            return compraDetalles;
        }

        [HttpGet("GetByCompraAndProducto")] // api/CompraDetalles/GetByCompraAndProducto?compraId=1&productoId=2
        public async Task<ActionResult<CompraDetalle>> GetByCompraAndProducto([FromQuery] int compraId, [FromQuery] int productoId)
        {
            CompraDetalle? compraDetalle = await repositorio.SelectByCompraAndProducto(compraId, productoId);
            if (compraDetalle == null)
            {
                return NotFound();
            }
            return compraDetalle;
        }

        [HttpGet("GetByRangoFechas")] // api/CompraDetalles/GetByRangoFechas?fechaInicio=2024-01-01&fechaFin=2024-01-31
        public async Task<ActionResult<List<CompraDetalle>>> GetByRangoFechas([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            // Validar que la fecha de inicio no sea mayor que la fecha fin
            if (fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");
            }

            var compraDetalles = await repositorio.SelectByRangoFechas(fechaInicio, fechaFin);
            return compraDetalles;
        }

        [HttpGet("existe/{id:int}")] // api/CompraDetalles/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("existeCombinacion")] // api/CompraDetalles/existeCombinacion?compraId=1&productoId=2
        public async Task<ActionResult<bool>> ExisteCombinacion([FromQuery] int compraId, [FromQuery] int productoId)
        {
            return await repositorio.Existe(compraId, productoId);
        }

        [HttpGet("total-por-compra/{compraId:int}")] // api/CompraDetalles/total-por-compra/1
        public async Task<ActionResult<decimal>> ObtenerTotalPorCompra(int compraId)
        {
            var total = await repositorio.ObtenerTotalPorCompra(compraId);
            return total;
        }

        [HttpGet("cantidad-total-por-producto/{productoId:int}")] // api/CompraDetalles/cantidad-total-por-producto/1
        public async Task<ActionResult<int>> ObtenerCantidadTotalPorProducto(int productoId)
        {
            var cantidad = await repositorio.ObtenerCantidadTotalPorProducto(productoId);
            return cantidad;
        }

        [HttpPost("actualizar-cantidad/{id:int}")] // api/CompraDetalles/actualizar-cantidad/2
        public async Task<ActionResult> ActualizarCantidad(int id, [FromBody] int cantidad)
        {
            try
            {
                var resultado = await repositorio.ActualizarCantidad(id, cantidad);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar la cantidad del detalle de compra");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("actualizar-precios/{id:int}")] // api/CompraDetalles/actualizar-precios/2
        public async Task<ActionResult> ActualizarPrecios(int id, [FromBody] ActualizarPreciosRequest request)
        {
            try
            {
                var resultado = await repositorio.ActualizarPrecios(id, request.PrecioCompra, request.PrecioVentaActualizado);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar los precios del detalle de compra");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CompraDetalle entidad)
        {
            try
            {
                // Verificar si ya existe el producto en la compra
                // (El repositorio maneja esto automáticamente sumando las cantidades)

                return await repositorio.Insert(entidad);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")] // api/CompraDetalles/2
        public async Task<ActionResult> Put(int id, [FromBody] CompraDetalle entidad)
        {
            try
            {
                if (id != entidad.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                var resultado = await repositorio.Update(id, entidad);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el detalle de compra");
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/CompraDetalles/2
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await repositorio.Delete(id);
            if (!resp)
            {
                return BadRequest("El detalle de compra no se pudo borrar");
            }
            return Ok();
        }

        [HttpDelete("DeleteByCompraAndProducto")] // api/CompraDetalles/DeleteByCompraAndProducto?compraId=1&productoId=2
        public async Task<ActionResult> DeleteByCompraAndProducto([FromQuery] int compraId, [FromQuery] int productoId)
        {
            var resp = await repositorio.DeleteByCompraAndProducto(compraId, productoId);
            if (!resp)
            {
                return BadRequest("El detalle de compra no se pudo borrar");
            }
            return Ok();
        }
    }

    // Clases auxiliares para requests
    public class ActualizarPreciosRequest
    {
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVentaActualizado { get; set; }
    }
}
