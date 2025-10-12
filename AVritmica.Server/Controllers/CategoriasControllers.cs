using Microsoft.AspNetCore.Mvc;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/Categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio repositorio;

        public CategoriasController(ICategoriaRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/Categorias
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener una categoría por ID
        /// </summary>
        /// <param name="id">Id de la categoría</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            Categoria? categoria = await repositorio.SelectById(id);  // ✅ Ahora reconoce que puede ser null
            if (categoria == null)
            {
                return NotFound();
            }
            return categoria;
        }

        [HttpGet("GetByNombre/{nombre}")] // api/Categorias/GetByNombre/Electrónica
        public async Task<ActionResult<Categoria>> GetByNombre(string nombre)
        {
            Categoria? categoria = await repositorio.SelectByNombre(nombre);
            if (categoria == null)
            {
                return NotFound();
            }
            return categoria;
        }

        [HttpGet("existe/{id:int}")] // api/Categorias/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("existeNombre/{nombre}")] // api/Categorias/existeNombre/Electrónica
        public async Task<ActionResult<bool>> ExisteNombre(string nombre)
        {
            return await repositorio.Existe(nombre);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Categoria entidad)
        {
            try
            {
                // Verificar si ya existe una categoría con el mismo nombre
                if (await repositorio.Existe(entidad.Nombre))
                {
                    return BadRequest("Ya existe una categoría con ese nombre");
                }

                return await repositorio.Insert(entidad);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")] // api/Categorias/2
        public async Task<ActionResult> Put(int id, [FromBody] Categoria entidad)
        {
            try
            {
                if (id != entidad.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                // Verificar si el nombre ya existe en otra categoría
                var categoriaExistente = await repositorio.SelectByNombre(entidad.Nombre);
                if (categoriaExistente != null && categoriaExistente.Id != id)
                {
                    return BadRequest("Ya existe otra categoría con ese nombre");
                }

                var resultado = await repositorio.Update(id, entidad);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar la categoría");
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/Categorias/2
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await repositorio.Delete(id);
            if (!resp)
            {
                return BadRequest("La categoría no se pudo borrar");
            }
            return Ok();
        }
    }
}
