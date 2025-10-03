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
    [Route("api/Consultas")]
    public class ConsultasControllers : ControllerBase
    {
        //private readonly Context context;
        private readonly IConsultaRepositorio repositorio;
        private readonly IMapper mapper;

        public ConsultasControllers(IConsultaRepositorio repositorio,
                                    IMapper mapper)
        {
            //this.context = context;
            this.repositorio = repositorio;
            this.mapper = mapper;
        }

        // GET: api/Consultas
        [HttpGet]
        public async Task<ActionResult<List<Consulta>>> Get()
        {
            return await repositorio.Select();
        }

        // GET: api/Consultas/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Consulta>> Get(int id)
        {
            Consulta? con = await repositorio.SelectById(id);

            if (con == null)
            {
                return NotFound();
            }

            return con;
        }

        [HttpGet("existe/{id:int}")] //api/Consultas/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await repositorio.Existe(id);
            return existe;
        }

        // POST: api/Consultas
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearConsultaDTO entidadDTO)
        {
            try
            {
                //Consulta entidad = new Consulta();
                //entidad.Nombre = entidadDTO.Nombre;
                //entidad.Email = entidadDTO.Email;
                //entidad.Mensaje = entidadDTO.Mensaje;
                //entidad.FechaEnvio = entidadDTO.FechaEnvio;

                Consulta entidad = mapper.Map<Consulta>(entidadDTO);
                //context.Consultas.Add(entidad);
                //await context.SaveChangesAsync();
                return await repositorio.Insert(entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // PUT: api/Consultas/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Consulta entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrrectos");
            }
            //var con = await context.Consultas
            //              .Where(e => e.Id == id)
            //              .FirstOrDefaultAsync();
            var con = await repositorio.SelectById(id);

            if (con == null)
            {
                return NotFound("No existe la consulta buscada.");
            }

            con.UsuarioId = entidad.UsuarioId;
            con.Nombre = entidad.Nombre;
            con.Email = entidad.Email;
            con.Mensaje = entidad.Mensaje;
            con.FechaEnvio = entidad.FechaEnvio;
            con.Activo = entidad.Activo;

            try
            {
                await repositorio.Update(id, con);
                //context.Consultas.Update(con);
                //await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Consultas/2
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await repositorio.Existe(id);
            if (!existe)
            {
                return NotFound($"La consulta {id} no existe.");
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
