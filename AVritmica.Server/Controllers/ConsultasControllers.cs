using Microsoft.AspNetCore.Mvc;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;
using AVritmica.Shared.DTO;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaRepositorio _consultaRepositorio;

        public ConsultasController(IConsultaRepositorio consultaRepositorio)
        {
            _consultaRepositorio = consultaRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<Consulta>>> GetConsultas()
        {
            try
            {
                var consultas = await _consultaRepositorio.Select();
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener consultas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> GetConsulta(int id)
        {
            try
            {
                var consulta = await _consultaRepositorio.SelectById(id);
                if (consulta == null)
                    return NotFound();

                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener consulta: {ex.Message}");
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<List<Consulta>>> GetConsultasPorUsuario(int usuarioId)
        {
            try
            {
                var consultas = await _consultaRepositorio.SelectByUsuario(usuarioId);
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener consultas del usuario: {ex.Message}");
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<List<Consulta>>> GetConsultasPorEmail(string email)
        {
            try
            {
                var consultas = await _consultaRepositorio.SelectByEmail(email);
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener consultas por email: {ex.Message}");
            }
        }

        [HttpGet("no-leidas")]
        public async Task<ActionResult<List<Consulta>>> GetConsultasNoLeidas()
        {
            try
            {
                var consultas = await _consultaRepositorio.SelectNoLeidas();
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener consultas no leídas: {ex.Message}");
            }
        }

        [HttpGet("cantidad-no-leidas")]
        public async Task<ActionResult<int>> GetCantidadNoLeidas()
        {
            try
            {
                var cantidad = await _consultaRepositorio.ObtenerCantidadNoLeidas();
                return Ok(cantidad);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener cantidad de consultas no leídas: {ex.Message}");
            }
        }

        // ENDPOINT ORIGINAL (para compatibilidad)
        [HttpPost]
        public async Task<ActionResult<Consulta>> PostConsulta(Consulta consulta)
        {
            try
            {
                var id = await _consultaRepositorio.Insert(consulta);
                var consultaCreada = await _consultaRepositorio.SelectById(id);
                return CreatedAtAction(nameof(GetConsulta), new { id }, consultaCreada);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la consulta: {ex.Message}");
            }
        }

        // NUEVO ENDPOINT: Para consultas públicas (sin usuario)
        [HttpPost("publica")]
        public async Task<ActionResult<Consulta>> PostConsultaPublica(CrearConsultaDTO consultaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var id = await _consultaRepositorio.InsertConsultaPublica(
                    consultaDTO.Nombre,
                    consultaDTO.Email,
                    consultaDTO.Mensaje
                );

                var consultaCreada = await _consultaRepositorio.SelectById(id);
                return CreatedAtAction(nameof(GetConsulta), new { id }, consultaCreada);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la consulta: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsulta(int id, Consulta consulta)
        {
            try
            {
                if (id != consulta.Id)
                    return BadRequest("ID mismatch");

                var resultado = await _consultaRepositorio.Update(id, consulta);
                if (!resultado)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar consulta: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsulta(int id)
        {
            try
            {
                var resultado = await _consultaRepositorio.Delete(id);
                if (!resultado)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar consulta: {ex.Message}");
            }
        }

        [HttpPost("marcar-leida/{id}")]
        public async Task<IActionResult> MarcarComoLeida(int id)
        {
            try
            {
                var resultado = await _consultaRepositorio.MarcarComoLeida(id);
                if (!resultado)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al marcar consulta como leída: {ex.Message}");
            }
        }
    }
}
