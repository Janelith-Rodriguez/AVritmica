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
    [Route("api/Productos")]
    public class ProductosControllers : ControllerBase
    {
        //private readonly Context context;
        private readonly IProductoRepositorio repositorio;
        private readonly IMapper mapper;

        public ProductosControllers(IProductoRepositorio repositorio,
                                    IMapper mapper)
        {
            //this.context = context;
            this.repositorio = repositorio;
            this.mapper = mapper;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> Get()
        {
            return await repositorio.Select();
        }

        // GET: api/Productos/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Producto>> Get(int id)
        {
            Producto? P = await repositorio.SelectById(id);

            if (P == null)
            {
                return NotFound();
            }

            return P;
        }

        [HttpGet("existe/{id:int}")] //api/Productos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await repositorio.Existe(id);
            return existe;
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearProductoDTO entidadDTO)
        {
            try
            {
                //Producto entidad = new Producto();
                //entidad.Nombre = entidadDTO.Nombre;
                //entidad.Descripcion = entidadDTO.Descripcion;
                //entidad.Precio = entidadDTO.Precio;
                //entidad.Stock = entidadDTO.Stock;
                //entidad.ImagenUrl = entidadDTO.ImagenUrl;

                Producto entidad = mapper.Map<Producto>(entidadDTO);
                //context.Productos.Add(entidad);
                //await context.SaveChangesAsync();
                return await repositorio.Insert(entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // PUT: api/Productos/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Producto entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrrectos");
            }
            //var P = await context.Productos
            //              .Where(e => e.Id == id)
            //              .FirstOrDefaultAsync();
            var P = await repositorio.SelectById(id);

            if (P == null)
            {
                return NotFound("No existe el producto buscado.");
            }

            P.Nombre = entidad.Nombre;
            P.Descripcion = entidad.Descripcion;
            P.Precio = entidad.Precio;
            P.Stock = entidad.Stock;
            P.ImagenUrl = entidad.ImagenUrl;
            P.CategoriaId = entidad.CategoriaId;
            P.Activo = entidad.Activo;

            try
            {
                await repositorio.Update(id, P);
                //context.Productos.Update(P);
                //await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Categorias/2
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await repositorio.Existe(id);
            if (!existe)
            {
                return NotFound($"El producto {id} no existe.");
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
