using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/Consultas")]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaRepositorio repositorio;

        public ConsultasController(IConsultaRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/Consultas
        public async Task<ActionResult<List<Consulta>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener una consulta por ID
        /// </summary>
        /// <param name="id">Id de la consulta</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] // api/Consultas/2
        public async Task<ActionResult<Consulta>> Get(int id)
        {
            Consulta? consulta = await repositorio.SelectById(id);
            if (consulta == null)
            {
                return NotFound();
            }
            return consulta;
        }

        [HttpGet("GetByUsuario/{usuarioId:int}")] // api/Consultas/GetByUsuario/1
        public async Task<ActionResult<List<Consulta>>> GetByUsuario(int usuarioId)
        {
            var consultas = await repositorio.SelectByUsuario(usuarioId);
            return consultas;
        }

        [HttpGet("GetByEmail/{email}")] // api/Consultas/GetByEmail/usuario@ejemplo.com
        public async Task<ActionResult<List<Consulta>>> GetByEmail(string email)
        {
            var consultas = await repositorio.SelectByEmail(email);
            return consultas;
        }

        [HttpGet("GetByRangoFechas")] // api/Consultas/GetByRangoFechas?fechaInicio=2024-01-01&fechaFin=2024-01-31
        public async Task<ActionResult<List<Consulta>>> GetByRangoFechas([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            // Validar que la fecha de inicio no sea mayor que la fecha fin
            if (fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");
            }

            var consultas = await repositorio.SelectByRangoFechas(fechaInicio, fechaFin);
            return consultas;
        }

        [HttpGet("GetNoLeidas")] // api/Consultas/GetNoLeidas
        public async Task<ActionResult<List<Consulta>>> GetNoLeidas()
        {
            var consultas = await repositorio.SelectNoLeidas();
            return consultas;
        }

        [HttpGet("existe/{id:int}")] // api/Consultas/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("cantidad-no-leidas")] // api/Consultas/cantidad-no-leidas
        public async Task<ActionResult<int>> ObtenerCantidadNoLeidas()
        {
            var cantidad = await repositorio.ObtenerCantidadNoLeidas();
            return cantidad;
        }

        [HttpPost("marcar-leida/{id:int}")] // api/Consultas/marcar-leida/2
        public async Task<ActionResult> MarcarComoLeida(int id)
        {
            try
            {
                var resultado = await repositorio.MarcarComoLeida(id);
                if (!resultado)
                {
                    return BadRequest("No se pudo marcar la consulta como leída");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Consulta entidad)
        {
            try
            {
                // Validar el formato del email
                if (!IsValidEmail(entidad.Email))
                {
                    return BadRequest("El formato del email no es válido");
                }

                // Validar que los campos requeridos no estén vacíos
                if (string.IsNullOrWhiteSpace(entidad.Nombre) ||
                    string.IsNullOrWhiteSpace(entidad.Mensaje))
                {
                    return BadRequest("El nombre y el mensaje son obligatorios");
                }

                return await repositorio.Insert(entidad);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")] // api/Consultas/2
        public async Task<ActionResult> Put(int id, [FromBody] Consulta entidad)
        {
            try
            {
                if (id != entidad.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                // Validar el formato del email
                if (!IsValidEmail(entidad.Email))
                {
                    return BadRequest("El formato del email no es válido");
                }

                // Validar que los campos requeridos no estén vacíos
                if (string.IsNullOrWhiteSpace(entidad.Nombre) ||
                    string.IsNullOrWhiteSpace(entidad.Mensaje))
                {
                    return BadRequest("El nombre y el mensaje son obligatorios");
                }

                var resultado = await repositorio.Update(id, entidad);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar la consulta");
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/Consultas/2
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await repositorio.Delete(id);
            if (!resp)
            {
                return BadRequest("La consulta no se pudo borrar");
            }
            return Ok();
        }

        /// <summary>
        /// Endpoint para enviar una consulta sin requerir autenticación
        /// </summary>
        [HttpPost("enviar-consulta")]
        public async Task<ActionResult<int>> EnviarConsulta([FromBody] EnviarConsultaRequest request)
        {
            try
            {
                // Validar el formato del email
                if (!IsValidEmail(request.Email))
                {
                    return BadRequest("El formato del email no es válido");
                }

                // Validar que los campos requeridos no estén vacíos
                if (string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.Mensaje))
                {
                    return BadRequest("El nombre y el mensaje son obligatorios");
                }

                var consulta = new Consulta
                {
                    UsuarioId = request.UsuarioId, // Puede ser 0 si no hay usuario autenticado
                    Nombre = request.Nombre,
                    Email = request.Email,
                    Mensaje = request.Mensaje,
                    FechaEnvio = DateTime.UtcNow
                };

                return await repositorio.Insert(consulta);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        // Método auxiliar para validar email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

    // Clases auxiliares para requests
    public class EnviarConsultaRequest
    {
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Mensaje { get; set; } = string.Empty;
    }
}
