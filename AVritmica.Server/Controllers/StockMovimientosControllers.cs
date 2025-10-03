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
    [Route("api/StockMovimientos")]
    public class StockMovimientosControllers : ControllerBase
    {
        //private readonly Context context;
        private readonly IStockMovimientoRepositorio repositorio;
        private readonly IMapper mapper;

        public StockMovimientosControllers(IStockMovimientoRepositorio repositorio,
                                           IMapper mapper)
        {
            //this.context = context;
            this.repositorio = repositorio;
            this.mapper = mapper;
        }

        // GET: api/StockMovimientos
        [HttpGet]
        public async Task<ActionResult<List<StockMovimiento>>> Get()
        {
            return await repositorio.Select();
        }

        // GET: api/StockMovimientos/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StockMovimiento>> Get(int id)
        {
            StockMovimiento? SM = await repositorio.SelectById(id);

            if (SM == null)
            {
                return NotFound();
            }

            return SM;
        }

        [HttpGet("existe/{id:int}")] //api/StockMovimientos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await repositorio.Existe(id);
            return existe;
        }

        // POST: api/StockMovimientos
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearStockMovimientoDTO entidadDTO)
        {
            try
            {
                //StockMovimiento entidad = new StockMovimiento();
                //entidad.TipoMovimiento = entidadDTO.TipoMovimiento;
                //entidad.Cantidad = entidadDTO.Cantidad;
                //entidad.Fecha = entidadDTO.Fecha;
                //entidad.Descripcion = entidadDTO.Descripcion;

                StockMovimiento entidad = mapper.Map<StockMovimiento>(entidadDTO);
                //context.StockMovimientos.Add(entidad);
                //await context.SaveChangesAsync();
                return await repositorio.Insert(entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // PUT: api/StockMovimientos/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] StockMovimiento entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrrectos");
            }
            //var SM = await context.StockMovimientos
            //              .Where(e => e.Id == id)
            //              .FirstOrDefaultAsync();
            var SM = await repositorio.SelectById(id);

            if (SM == null)
            {
                return NotFound("No existe el stock movimiento buscada.");
            }

            SM.ProductoId = entidad.ProductoId;
            SM.TipoMovimiento = entidad.TipoMovimiento;
            SM.Cantidad = entidad.Cantidad;
            SM.Fecha = entidad.Fecha;
            SM.Descripcion = entidad.Descripcion;
            SM.Activo = entidad.Activo;

            try
            {
                await repositorio.Update(id, SM);
                //context.StockMovimientos.Update(SM);
                //await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/StockMovimientos/2
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await repositorio.Existe(id);
            if (!existe)
            {
                return NotFound($"El stock movimiento {id} no existe.");
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
