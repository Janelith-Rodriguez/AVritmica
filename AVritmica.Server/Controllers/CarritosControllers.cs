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
    [Route("api/Carritos")]
    public class CarritosControllers : ControllerBase
    {
        //private readonly Context context;
        private readonly ICarritoRepositorio repositorio;
        private readonly IMapper mapper;

        public CarritosControllers(ICarritoRepositorio repositorio,
                                   IMapper mapper)
        {
            //this.context = context;
            this.repositorio = repositorio;
            this.mapper = mapper;
        }

        // GET: api/Carritos
        [HttpGet]
        public async Task<ActionResult<List<Carrito>>> Get()
        {
            return await repositorio.Select();
        }

        // GET: api/Carritos/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Carrito>> Get(int id)
        {
            Carrito? car = await repositorio.SelectById(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        [HttpGet("existe/{id:int}")] //api/Carritos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await repositorio.Existe(id);
            return existe;
        }

        // POST: api/Carritos
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCarritoDTO entidadDTO)
        {
            try
            {
                //Carrito entidad = new Carrito();
                //entidad.FechaCreacion = entidadDTO.FechaCreacion;
                //entidad.Estado = entidadDTO.Estado;
                //entidad.FechaConfirmacion = entidadDTO.FechaConfirmacion;
                //entidad.EstadoPago = entidadDTO.EstadoPago;
                //entidad.MontoTotal = entidadDTO.MontoTotal;
                //entidad.Saldo = entidadDTO.Saldo;
                //entidad.DireccionEnvio = entidadDTO.DireccionEnvio;

                Carrito entidad = mapper.Map<Carrito>(entidadDTO);
                //context.Carritos.Add(entidad);
                //await context.SaveChangesAsync();
                return await repositorio.Insert(entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // PUT: api/Carritos/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Carrito entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrrectos");
            }
            //var car = await context.Carritos
            //              .Where(e => e.Id == id)
            //              .FirstOrDefaultAsync();
            var car = await repositorio.SelectById(id);

            if (car == null)
            {
                return NotFound("No existe el carrito buscado.");
            }

            car.UsuarioId = entidad.UsuarioId;
            car.FechaCreacion = entidad.FechaCreacion;
            car.Estado = entidad.Estado;
            car.FechaConfirmacion = entidad.FechaConfirmacion;
            car.EstadoPago = entidad.EstadoPago;
            car.MontoTotal = entidad.MontoTotal;
            car.Saldo = entidad.Saldo;
            car.DireccionEnvio = entidad.DireccionEnvio; 
            car.Activo = entidad.Activo;

            try
            {
                await repositorio.Update(id, car);
                //context.Carritos.Update(car);
                //await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Carritos/2
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await repositorio.Existe(id);
            if (!existe)
            {
                return NotFound($"El carrito {id} no existe.");
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
