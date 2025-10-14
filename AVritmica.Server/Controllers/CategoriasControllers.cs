using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _repositorio;

        public CategoriasController(ICategoriaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            var categorias = await _repositorio.Select();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            var categoria = await _repositorio.SelectById(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Categoria categoria)
        {
            try
            {
                var id = await _repositorio.Insert(categoria);
                return CreatedAtAction(nameof(Get), new { id = id }, categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            var resultado = await _repositorio.Update(id, categoria);
            if (!resultado)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var resultado = await _repositorio.Delete(id);
            if (!resultado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
