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
    [Route("api/Pagos")]
    public class PagosControllers : ControllerBase
    {
        //private readonly Context context;
        private readonly IPagoRepositorio repositorio;
        private readonly IMapper mapper;

        public PagosControllers(IPagoRepositorio repositorio,
                                IMapper mapper)
        {
            //this.context = context;
            this.repositorio = repositorio;
            this.mapper = mapper;
        }

        // GET: api/Pagos
        [HttpGet]
        public async Task<ActionResult<List<Pago>>> Get()
        {
            return await repositorio.Select();
        }

        // GET: api/Pagos/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Pago>> Get(int id)
        {
            Pago? pa = await repositorio.SelectById(id);

            if (pa == null)
            {
                return NotFound();
            }

            return pa;
        }

        [HttpGet("existe/{id:int}")] //api/Pagos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await repositorio.Existe(id);
            return existe;
        }

        // POST: api/Categorias
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearPagoDTO entidadDTO)
        {
            try
            {
                //Pago entidad = new Pago();
                //entidad.FechaPago = entidadDTO.FechaPago;
                //entidad.MetodoPago = entidadDTO.MetodoPago;
                //entidad.MontoPagado = entidadDTO.MontoPagado;
                //entidad.EstadoPago = entidadDTO.EstadoPago;
                //entidad.Saldo = entidadDTO.Saldo;

                Pago entidad = mapper.Map<Pago>(entidadDTO);
                //context.Pagos.Add(entidad);
                //await context.SaveChangesAsync();
                return await repositorio.Insert(entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // PUT: api/Pagos/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Pago entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrrectos");
            }
            //var pa = await context.Pagos
            //              .Where(e => e.Id == id)
            //              .FirstOrDefaultAsync();
            var pa = await repositorio.SelectById(id);

            if (pa == null)
            {
                return NotFound("No existe el pago buscado.");
            }

            pa.CarritoId = entidad.CarritoId;
            pa.FechaPago = entidad.FechaPago;
            pa.MetodoPago = entidad.MetodoPago;
            pa.MontoPagado = entidad.MontoPagado;
            pa.EstadoPago = entidad.EstadoPago;
            pa.Saldo = entidad.Saldo;
            pa.Activo = entidad.Activo;

            try
            {
                await repositorio.Update(id, pa);
                //context.Pagos.Update(pa);
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
                return NotFound($"El pago {id} no existe.");
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
