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
    [Route("api/CompraDetalles")]

    public class CompraDetallesControllers : ControllerBase
    {
        //private readonly Context context;
        private readonly ICompraDetalleRepositorio repositorio;
        private readonly IMapper mapper;

        public CompraDetallesControllers(ICompraDetalleRepositorio repositorio,
                                         IMapper mapper)
        {
            //this.context = context;
            this.repositorio = repositorio;
            this.mapper = mapper;
        }

        // GET: api/CompraDetalles
        [HttpGet]
        public async Task<ActionResult<List<CompraDetalle>>> Get()
        {
            return await repositorio.Select();
        }

        // GET: api/CompraDetalles/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompraDetalle>> Get(int id)
        {
            CompraDetalle? cd = await repositorio.SelectById(id);

            if (cd == null)
            {
                return NotFound();
            }

            return cd;
        }

        [HttpGet("existe/{id:int}")] //api/CompraDetalles/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await repositorio.Existe(id);
            return existe;
        }

        // POST: api/CompraDetalles
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCompraDetalleDTO entidadDTO)
        {
            try
            {
                //CompraDetalle entidad = new CompraDetalle();
                //entidad.Cantidad = entidadDTO.Cantidad;
                //entidad.PrecioCompra = entidadDTO.PrecioCompra;
                //entidad.PrecioVentaActualizado = entidadDTO.PrecioVentaActualizado;

                CompraDetalle entidad = mapper.Map<CompraDetalle>(entidadDTO);
                //context.CompraDetalles.Add(entidad);
                //await context.SaveChangesAsync();
                return await repositorio.Insert(entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // PUT: api/CompraDetalles/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CompraDetalle entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrrectos");
            }
            //var cd = await context.CompraDetalles
            //              .Where(e => e.Id == id)
            //              .FirstOrDefaultAsync();
            var cd = await repositorio.SelectById(id);

            if (cd == null)
            {
                return NotFound("No existe la compra detalle buscada.");
            }

            cd.CompraId = entidad.CompraId;
            cd.ProductoId = entidad.ProductoId;
            cd.Cantidad = entidad.Cantidad;
            cd.PrecioCompra = entidad.PrecioCompra;
            cd.PrecioVentaActualizado = entidad.PrecioVentaActualizado;
            cd.Activo = entidad.Activo;

            try
            {
                await repositorio.Update(id, cd);
                //context.CompraDetalles.Update(cd);
                //await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/CompraDetalles/2
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await repositorio.Existe(id);
            if (!existe)
            {
                return NotFound($"La compra detalle {id} no existe.");
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
