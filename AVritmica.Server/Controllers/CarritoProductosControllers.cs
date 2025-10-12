using Microsoft.AspNetCore.Mvc;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/CarritoProductos")]
    public class CarritoProductosController : ControllerBase
    {
        private readonly ICarritoProductoRepositorio repositorio;

        public CarritoProductosController(ICarritoProductoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/CarritoProductos
        public async Task<ActionResult<List<CarritoProducto>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener un producto del carrito por ID
        /// </summary>
        /// <param name="id">Id del carrito producto</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] // api/CarritoProductos/2
        public async Task<ActionResult<CarritoProducto>> Get(int id)
        {
            CarritoProducto? carritoProducto = await repositorio.SelectById(id);
            if (carritoProducto == null)
            {
                return NotFound();
            }
            return carritoProducto;
        }

        [HttpGet("GetByCarrito/{carritoId:int}")] // api/CarritoProductos/GetByCarrito/1
        public async Task<ActionResult<List<CarritoProducto>>> GetByCarrito(int carritoId)
        {
            var carritoProductos = await repositorio.SelectByCarrito(carritoId);
            return carritoProductos;
        }

        [HttpGet("GetByProducto/{productoId:int}")] // api/CarritoProductos/GetByProducto/1
        public async Task<ActionResult<List<CarritoProducto>>> GetByProducto(int productoId)
        {
            var carritoProductos = await repositorio.SelectByProducto(productoId);
            return carritoProductos;
        }

        [HttpGet("GetByCarritoAndProducto")] // api/CarritoProductos/GetByCarritoAndProducto?carritoId=1&productoId=2
        public async Task<ActionResult<CarritoProducto>> GetByCarritoAndProducto([FromQuery] int carritoId, [FromQuery] int productoId)
        {
            CarritoProducto? carritoProducto = await repositorio.SelectByCarritoAndProducto(carritoId, productoId);
            if (carritoProducto == null)
            {
                return NotFound();
            }
            return carritoProducto;
        }

        [HttpGet("existe/{id:int}")] // api/CarritoProductos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("existeCombinacion")] // api/CarritoProductos/existeCombinacion?carritoId=1&productoId=2
        public async Task<ActionResult<bool>> ExisteCombinacion([FromQuery] int carritoId, [FromQuery] int productoId)
        {
            return await repositorio.Existe(carritoId, productoId);
        }

        [HttpGet("cantidadTotalEnCarritos/{productoId:int}")] // api/CarritoProductos/cantidadTotalEnCarritos/1
        public async Task<ActionResult<int>> CantidadTotalEnCarritos(int productoId)
        {
            var cantidad = await repositorio.ObtenerCantidadTotalEnCarritos(productoId);
            return cantidad;
        }

        [HttpPost("actualizar-cantidad/{id:int}")] // api/CarritoProductos/actualizar-cantidad/2
        public async Task<ActionResult> ActualizarCantidad(int id, [FromBody] int cantidad)
        {
            try
            {
                var resultado = await repositorio.ActualizarCantidad(id, cantidad);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar la cantidad del producto en el carrito");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("actualizar-precio/{id:int}")] // api/CarritoProductos/actualizar-precio/2
        public async Task<ActionResult> ActualizarPrecioUnitario(int id, [FromBody] decimal precioUnitario)
        {
            try
            {
                var resultado = await repositorio.ActualizarPrecioUnitario(id, precioUnitario);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el precio unitario del producto en el carrito");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CarritoProducto entidad)
        {
            try
            {
                // Verificar si ya existe el producto en el carrito
                // (El repositorio maneja esto automáticamente sumando las cantidades)

                return await repositorio.Insert(entidad);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")] // api/CarritoProductos/2
        public async Task<ActionResult> Put(int id, [FromBody] CarritoProducto entidad)
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
                    return BadRequest("No se pudo actualizar el producto del carrito");
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/CarritoProductos/2
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await repositorio.Delete(id);
            if (!resp)
            {
                return BadRequest("El producto del carrito no se pudo borrar");
            }
            return Ok();
        }

        [HttpDelete("DeleteByCarritoAndProducto")] // api/CarritoProductos/DeleteByCarritoAndProducto?carritoId=1&productoId=2
        public async Task<ActionResult> DeleteByCarritoAndProducto([FromQuery] int carritoId, [FromQuery] int productoId)
        {
            var resp = await repositorio.DeleteByCarritoAndProducto(carritoId, productoId);
            if (!resp)
            {
                return BadRequest("El producto del carrito no se pudo borrar");
            }
            return Ok();
        }
    }
}
