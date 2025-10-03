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
    [Route("api/Categorias")]
    public class CategoriasControllers : ControllerBase
    {
        private readonly ICategoriaRepositorio repositorio;
        //private readonly Context context;
        private readonly IMapper mapper;

        public CategoriasControllers(ICategoriaRepositorio repositorio,
                                     IMapper mapper)
        {
            this.repositorio = repositorio;
            //this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            return await repositorio.Select();
        }

        // GET: api/Categorias/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            Categoria? C = await repositorio.SelectById(id);

            if (C == null)
            {
                return NotFound();
            }

            return C;
        }

        [HttpGet("existe/{id:int}")] //api/Categorias/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await repositorio.Existe(id);
            return existe;
        }

        // POST: api/Categorias
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCategoriaDTO entidadDTO)
        {
            try
            {
                //Categoria entidad = new Categoria();
                //entidad.Nombre = entidadDTO.Nombre;
                //entidad.Descripcion = entidadDTO.Descripcion;

                Categoria entidad = mapper.Map<Categoria>(entidadDTO);
                //context.Categorias.Add(entidad);
                //await context.SaveChangesAsync();
                return await repositorio.Insert (entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // PUT: api/Categorias/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Categoria entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrrectos");
            }
            //var C = await context.Categorias
            //              .Where(e => e.Id == id)
            //              .FirstOrDefaultAsync();
            var C = await repositorio.SelectById(id);

            if (C == null)
            {
                return NotFound("No existe la categoria buscada.");
            }

            C.Nombre = entidad.Nombre;
            C.Descripcion = entidad.Descripcion;
            C.Activo = entidad.Activo;

            try
            {
                await repositorio.Update(id, C);
                //context.Categorias.Update(C);
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
                return NotFound($"La categoria {id} no existe.");
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
