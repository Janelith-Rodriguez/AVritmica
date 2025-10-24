using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/Usuarios")]
    public class UsuariosControllers: ControllerBase
    {
        private readonly IUsuarioRepositorio repositorio;

        public UsuariosControllers(IUsuarioRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/Usuarios
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener un usuario por ID
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] // api/Usuarios/2
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            Usuario? usuario = await repositorio.SelectById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        [HttpGet("GetByEmail/{email}")] // api/Usuarios/GetByEmail/test@example.com
        public async Task<ActionResult<Usuario>> GetByEmail(string email)
        {
            Usuario? usuario = await repositorio.SelectByEmail(email);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        [HttpGet("GetByTipoUsuario/{tipoUsuario}")] // api/Usuarios/GetByTipoUsuario/Cliente
        public async Task<ActionResult<List<Usuario>>> GetByTipoUsuario(string tipoUsuario)
        {
            var usuarios = await repositorio.SelectByTipoUsuario(tipoUsuario);
            return usuarios;
        }

        [HttpGet("existe/{id:int}")] // api/Usuarios/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("existeEmail/{email}")] // api/Usuarios/existeEmail/test@example.com
        public async Task<ActionResult<bool>> ExisteEmail(string email)
        {
            return await repositorio.ExisteEmail(email);
        }

        [HttpPost("validar-credenciales")] // api/Usuarios/validar-credenciales
        public async Task<ActionResult<Usuario>> ValidarCredenciales([FromBody] LoginRequest request)
        {
            Usuario? usuario = await repositorio.ValidarCredenciales(request.Email, request.Contrasena);
            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas");
            }
            return usuario;
        }

        [HttpPost("actualizar-tipo-usuario/{id:int}")] // api/Usuarios/actualizar-tipo-usuario/2
        public async Task<ActionResult> ActualizarTipoUsuario(int id, [FromBody] string tipoUsuario)
        {
            try
            {
                var resultado = await repositorio.ActualizarTipoUsuario(id, tipoUsuario);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el tipo de usuario");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("actualizar-perfil/{id:int}")] // api/Usuarios/actualizar-perfil/2
        public async Task<ActionResult> ActualizarPerfil(int id, [FromBody] ActualizarPerfilRequest request)
        {
            try
            {
                var resultado = await repositorio.ActualizarPerfil(id, request.Nombre, request.Apellido, request.Telefono, request.Direccion);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el perfil");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("cambiar-contrasena/{id:int}")] // api/Usuarios/cambiar-contrasena/2
        public async Task<ActionResult> CambiarContrasena(int id, [FromBody] string nuevaContrasena)
        {
            try
            {
                var resultado = await repositorio.CambiarContrasena(id, nuevaContrasena);
                if (!resultado)
                {
                    return BadRequest("No se pudo cambiar la contraseña");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Usuario entidad)
        {
            try
            {
                return await repositorio.Insert(entidad);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")] // api/Usuarios/2
        public async Task<ActionResult> Put(int id, [FromBody] Usuario entidad)
        {
            try
            {
                if (id != entidad.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                var resultado = await repositorio.Update(id, entidad);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el usuario");
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/Usuarios/2
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var resp = await repositorio.Delete(id);
                if (!resp)
                {
                    return BadRequest("El usuario no se pudo borrar");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }

    // Clases auxiliares para los requests
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }

    public class ActualizarPerfilRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }
}
