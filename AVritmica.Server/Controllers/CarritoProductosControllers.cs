using AutoMapper;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;
using AVritmica.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/CarritoProductos")]
    public class CarritoProductosControllers : ControllerBase
    {
        private readonly ICarritoProductoRepositorio repositorio;
        //private readonly Context context;
        private readonly IMapper mapper;

        public CarritoProductosControllers(ICarritoProductoRepositorio repositorio, 
                                           IMapper mapper)
        {
            this.repositorio = repositorio;
            //this.context = context;
            this.mapper = mapper;
        }

        // GET: api/CarritoProductos
        [HttpGet]
        public async Task<ActionResult<List<CarritoProducto>>> Get()
        {
            return await repositorio.Select();
        }

        // GET: api/CarritoProductos/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CarritoProducto>> Get(int id)
        {
            CarritoProducto? cp = await repositorio.SelectById(id);

            if (cp == null)
            {
                return NotFound();
            }

            return cp;
        }

        [HttpGet("existe/{id:int}")] //api/CarritoProductos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await repositorio.Existe(id);
            return existe;
        }

        // POST: api/CarritoProductos
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCarritoProductoDTO entidadDTO)
        {
            try
            {
                //CarritoProducto entidad = new CarritoProducto();
                //entidad.Cantidad = entidadDTO.Cantidad;
                //entidad.PrecioUnitario = entidadDTO.PrecioUnitario;

                CarritoProducto entidad = mapper.Map<CarritoProducto>(entidadDTO);
                //context.CarritoProductos.Add(entidad);
                //await context.SaveChangesAsync();
                return await repositorio.Insert(entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // PUT: api/CarritoProductos/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CarritoProducto entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrrectos");
            }
            //var cp = await context.CarritoProductos
            //              .Where(e => e.Id == id)
            //              .FirstOrDefaultAsync();
            var cp = await repositorio.SelectById(id);

            if (cp == null)
            {
                return NotFound("No existe el carrito producto buscado.");
            }

            cp.CarritoId = entidad.CarritoId;
            cp.ProductoId = entidad.ProductoId;
            cp.Cantidad = entidad.Cantidad;
            cp.PrecioUnitario = entidad.PrecioUnitario;
            cp.Activo = entidad.Activo;

            try
            {
                await repositorio.Update(id, cp);
                //context.CarritoProductos.Update(cp);
                //await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/CarritoProductos/2
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await repositorio.Existe(id);
            if (!existe)
            {
                return NotFound($"El carrito producto {id} no existe.");
            }
            if (await repositorio.Delete(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
