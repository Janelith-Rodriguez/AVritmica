using Microsoft.AspNetCore.Mvc;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/Productos")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepositorio repositorio;

        public ProductosController(IProductoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/Productos
        public async Task<ActionResult<List<Producto>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener un producto por ID
        /// </summary>
        /// <param name="id">Id del producto</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] // api/Productos/2
        public async Task<ActionResult<Producto>> Get(int id)
        {
            Producto? producto = await repositorio.SelectById(id);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        [HttpGet("GetByNombre/{nombre}")] // api/Productos/GetByNombre/Laptop
        public async Task<ActionResult<Producto>> GetByNombre(string nombre)
        {
            Producto? producto = await repositorio.SelectByNombre(nombre);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        [HttpGet("GetByCategoria/{categoriaId:int}")] // api/Productos/GetByCategoria/1
        public async Task<ActionResult<List<Producto>>> GetByCategoria(int categoriaId)
        {
            var productos = await repositorio.SelectByCategoria(categoriaId);
            return productos;
        }

        [HttpGet("GetByPrecioRange")] // api/Productos/GetByPrecioRange?precioMin=100&precioMax=500
        public async Task<ActionResult<List<Producto>>> GetByPrecioRange([FromQuery] decimal precioMin, [FromQuery] decimal precioMax)
        {
            var productos = await repositorio.SelectByPrecioRange(precioMin, precioMax);
            return productos;
        }

        [HttpGet("existe/{id:int}")] // api/Productos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("existeNombre/{nombre}")] // api/Productos/existeNombre/Laptop
        public async Task<ActionResult<bool>> ExisteNombre(string nombre)
        {
            return await repositorio.Existe(nombre);
        }

        [HttpPost("actualizar-stock/{id:int}")] // api/Productos/actualizar-stock/2
        public async Task<ActionResult> ActualizarStock(int id, [FromBody] int cantidad)
        {
            try
            {
                var resultado = await repositorio.ActualizarStock(id, cantidad);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el stock del producto");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Producto entidad)
        {
            try
            {
                // Verificar si ya existe un producto con el mismo nombre
                if (await repositorio.Existe(entidad.Nombre))
                {
                    return BadRequest("Ya existe un producto con ese nombre");
                }

                return await repositorio.Insert(entidad);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")] // api/Productos/2
        public async Task<ActionResult> Put(int id, [FromBody] Producto entidad)
        {
            try
            {
                if (id != entidad.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                // Verificar si el nombre ya existe en otro producto
                var productoExistente = await repositorio.SelectByNombre(entidad.Nombre);
                if (productoExistente != null && productoExistente.Id != id)
                {
                    return BadRequest("Ya existe otro producto con ese nombre");
                }

                var resultado = await repositorio.Update(id, entidad);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el producto");
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/Productos/2
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await repositorio.Delete(id);
            if (!resp)
            {
                return BadRequest("El producto no se pudo borrar");
            }
            return Ok();
        }
    }
}
