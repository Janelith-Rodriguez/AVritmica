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
    [Route("api/Compras")]
    public class ComprasControllers : ControllerBase
    {
        //private readonly Context context;
        private readonly ICompraRepositorio repositorio;
        private readonly IMapper mapper;

        public ComprasControllers(ICompraRepositorio repositorio,
                                  IMapper mapper)
        {
            //this.context = context;
            this.repositorio = repositorio;
            this.mapper = mapper;
        }

        // GET: api/Compras
        [HttpGet]
        public async Task<ActionResult<List<Compra>>> Get()
        {
            return await repositorio.Select();
        }

        // GET: api/Compras/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Compra>> Get(int id)
        {
            Compra? com = await repositorio.SelectById(id);

            if (com == null)
            {
                return NotFound();
            }

            return com;
        }

        [HttpGet("existe/{id:int}")] //api/Compras/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await repositorio.Existe(id);
            return existe;
        }

        // POST: api/Compras
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCompraDTO entidadDTO)
        {
            try
            {
                //Compra entidad = new Compra();
                //entidad.FechaCompra = entidadDTO.FechaCompra;
                //entidad.Descripcion = entidadDTO.Descripcion;

                Compra entidad = mapper.Map<Compra>(entidadDTO);
                //context.Compras.Add(entidad);
                //await context.SaveChangesAsync();
                return await repositorio.Insert(entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // PUT: api/Compras/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Compra entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrrectos");
            }
            //var com = await context.Compras
            //              .Where(e => e.Id == id)
            //              .FirstOrDefaultAsync();
            var com = await repositorio.SelectById(id);

            if (com == null)
            {
                return NotFound("No existe la compra buscada.");
            }

            com.FechaCompra = entidad.FechaCompra;
            com.Descripcion = entidad.Descripcion;
            com.Activo = entidad.Activo;

            try
            {
                await repositorio.Update(id, com);
                //context.Compras.Update(com);
                //await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Compras/2
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await repositorio.Existe(id);
            if (!existe)
            {
                return NotFound($"La compra {id} no existe.");
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
