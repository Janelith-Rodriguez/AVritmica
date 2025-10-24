using Microsoft.AspNetCore.Mvc;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ActionResult<int>> Post(CompraDetalleDto dto)
        {
            try
            {
                // Validar manualmente el DTO
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entidad = new CompraDetalle
                {
                    CompraId = dto.CompraId,
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad,
                    PrecioCompra = dto.PrecioCompra,
                    PrecioVentaActualizado = dto.PrecioVentaActualizado
                };

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
        public async Task<ActionResult> Put(int id, [FromBody] CompraDetalleDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                // Validar manualmente el DTO
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entidad = new CompraDetalle
                {
                    Id = dto.Id,
                    CompraId = dto.CompraId,
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad,
                    PrecioCompra = dto.PrecioCompra,
                    PrecioVentaActualizado = dto.PrecioVentaActualizado
                };

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

        // Métodos adicionales para mejor experiencia de usuario
        [HttpGet("validar-combinacion")]
        public async Task<ActionResult<ValidacionResponse>> ValidarCombinacion([FromQuery] int compraId, [FromQuery] int productoId, [FromQuery] int? excludeId = null)
        {
            try
            {
                var existe = await repositorio.Existe(compraId, productoId);

                if (existe && excludeId.HasValue)
                {
                    // Si estamos editando, verificar si es el mismo registro
                    var existente = await repositorio.SelectByCompraAndProducto(compraId, productoId);
                    if (existente != null && existente.Id == excludeId.Value)
                    {
                        existe = false; // Es el mismo registro, permitir
                    }
                }

                return new ValidacionResponse
                {
                    Existe = existe,
                    Mensaje = existe ? "Ya existe un detalle con esta combinación de compra y producto" : "Combinación válida"
                };
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al validar combinación: {ex.Message}");
            }
        }

        // Método para obtener estadísticas rápidas
        [HttpGet("estadisticas-compra/{compraId:int}")]
        public async Task<ActionResult<EstadisticasCompraResponse>> GetEstadisticasCompra(int compraId)
        {
            try
            {
                var detalles = await repositorio.SelectByCompra(compraId);
                var totalCompra = await repositorio.ObtenerTotalPorCompra(compraId);

                return new EstadisticasCompraResponse
                {
                    TotalProductos = detalles.Count,
                    TotalUnidades = detalles.Sum(d => d.Cantidad),
                    TotalCompra = totalCompra,
                    Productos = detalles.Select(d => new ProductoResumen
                    {
                        Nombre = d.Producto?.Nombre ?? "N/A",
                        Cantidad = d.Cantidad,
                        Subtotal = d.Cantidad * d.PrecioCompra
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener estadísticas: {ex.Message}");
            }
        }
    }

    // Clases DTO para requests
    public class CompraDetalleDto
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

    public class EstadisticasCompraResponse
    {
        public int TotalProductos { get; set; }
        public int TotalUnidades { get; set; }
        public decimal TotalCompra { get; set; }
        public List<ProductoResumen> Productos { get; set; } = new();
    }

    public class ProductoResumen
    {
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
