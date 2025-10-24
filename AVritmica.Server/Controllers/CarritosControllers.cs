using Microsoft.AspNetCore.Mvc;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/Carritos")]
    public class CarritosController : ControllerBase
    {
        private readonly ICarritoRepositorio repositorio;

        public CarritosController(ICarritoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/Carritos
        public async Task<ActionResult<List<Carrito>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener un carrito por ID
        /// </summary>
        /// <param name="id">Id del carrito</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] // api/Carritos/2
        public async Task<ActionResult<Carrito>> Get(int id)
        {
            Carrito? carrito = await repositorio.SelectById(id);
            if (carrito == null)
            {
                return NotFound();
            }
            return carrito;
        }

        [HttpGet("GetByUsuario/{usuarioId:int}")] // api/Carritos/GetByUsuario/1
        public async Task<ActionResult<List<Carrito>>> GetByUsuario(int usuarioId)
        {
            var carritos = await repositorio.SelectByUsuario(usuarioId);
            return carritos;
        }

        [HttpGet("GetByEstado/{estado}")] // api/Carritos/GetByEstado/Activo
        public async Task<ActionResult<List<Carrito>>> GetByEstado(string estado)
        {
            var carritos = await repositorio.SelectByEstado(estado);
            return carritos;
        }

        [HttpGet("GetByEstadoPago/{estadoPago}")] // api/Carritos/GetByEstadoPago/Pendiente
        public async Task<ActionResult<List<Carrito>>> GetByEstadoPago(string estadoPago)
        {
            var carritos = await repositorio.SelectByEstadoPago(estadoPago);
            return carritos;
        }

        [HttpGet("GetCarritoActivo/{usuarioId:int}")] // api/Carritos/GetCarritoActivo/1
        public async Task<ActionResult<Carrito>> GetCarritoActivo(int usuarioId)
        {
            Carrito? carrito = await repositorio.SelectCarritoActivoByUsuario(usuarioId);
            if (carrito == null)
            {
                return NotFound();
            }
            return carrito;
        }

        [HttpGet("existe/{id:int}")] // api/Carritos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("existeCarritoActivo/{usuarioId:int}")] // api/Carritos/existeCarritoActivo/1
        public async Task<ActionResult<bool>> ExisteCarritoActivo(int usuarioId)
        {
            return await repositorio.ExisteCarritoActivo(usuarioId);
        }

        [HttpPost("actualizar-estado/{id:int}")] // api/Carritos/actualizar-estado/2
        public async Task<ActionResult> ActualizarEstado(int id, [FromBody] string estado)
        {
            try
            {
                var resultado = await repositorio.ActualizarEstado(id, estado);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el estado del carrito");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("actualizar-estado-pago/{id:int}")] // api/Carritos/actualizar-estado-pago/2
        public async Task<ActionResult> ActualizarEstadoPago(int id, [FromBody] string estadoPago)
        {
            try
            {
                var resultado = await repositorio.ActualizarEstadoPago(id, estadoPago);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el estado de pago del carrito");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("confirmar-carrito/{id:int}")] // api/Carritos/confirmar-carrito/2
        public async Task<ActionResult> ConfirmarCarrito(int id, [FromBody] ConfirmarCarritoRequest request)
        {
            try
            {
                var resultado = await repositorio.ConfirmarCarrito(id, request.MontoTotal, request.DireccionEnvio);
                if (!resultado)
                {
                    return BadRequest("No se pudo confirmar el carrito");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("actualizar-monto/{id:int}")] // api/Carritos/actualizar-monto/2
        public async Task<ActionResult> ActualizarMontoTotal(int id, [FromBody] decimal montoTotal)
        {
            try
            {
                var resultado = await repositorio.ActualizarMontoTotal(id, montoTotal);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el monto total del carrito");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Método POST original mantenido para compatibilidad
        [HttpPost]
        public async Task<ActionResult<int>> Post(Carrito entidad)
        {
            try
            {
                // Verificar si el usuario ya tiene un carrito activo
                if (entidad.Estado == "Activo" && await repositorio.ExisteCarritoActivo(entidad.UsuarioId))
                {
                    return BadRequest("El usuario ya tiene un carrito activo");
                }

                return await repositorio.Insert(entidad);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        // Nuevo método POST que acepta DTO
        [HttpPost("crear")]
        public async Task<ActionResult<int>> CrearCarrito([FromBody] CarritoCreateRequest request)
        {
            try
            {
                // Verificar si el usuario ya tiene un carrito activo
                if (request.Estado == "Activo" && await repositorio.ExisteCarritoActivo(request.UsuarioId))
                {
                    return BadRequest("El usuario ya tiene un carrito activo");
                }

                var carrito = new Carrito
                {
                    UsuarioId = request.UsuarioId,
                    Estado = request.Estado,
                    EstadoPago = request.EstadoPago,
                    MontoTotal = request.MontoTotal,
                    Saldo = request.Saldo,
                    DireccionEnvio = request.DireccionEnvio,
                    FechaCreacion = DateTime.UtcNow
                };

                return await repositorio.Insert(carrito);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        // Método PUT original mantenido para compatibilidad
        [HttpPut("{id:int}")] // api/Carritos/2
        public async Task<ActionResult> Put(int id, [FromBody] Carrito entidad)
        {
            try
            {
                if (id != entidad.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                // Validar que no se active otro carrito si ya existe uno activo
                if (entidad.Estado == "Activo")
                {
                    var carritoExistente = await repositorio.SelectCarritoActivoByUsuario(entidad.UsuarioId);
                    if (carritoExistente != null && carritoExistente.Id != id)
                    {
                        return BadRequest("El usuario ya tiene otro carrito activo");
                    }
                }

                var resultado = await repositorio.Update(id, entidad);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el carrito");
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Nuevo método PUT que acepta DTO
        [HttpPut("actualizar/{id:int}")]
        public async Task<ActionResult> ActualizarCarrito(int id, [FromBody] CarritoUpdateRequest request)
        {
            try
            {
                if (id != request.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                var carritoExistente = await repositorio.SelectById(id);
                if (carritoExistente == null)
                {
                    return NotFound();
                }

                // Validar que no se active otro carrito si ya existe uno activo
                if (request.Estado == "Activo")
                {
                    var carritoActivo = await repositorio.SelectCarritoActivoByUsuario(request.UsuarioId);
                    if (carritoActivo != null && carritoActivo.Id != id)
                    {
                        return BadRequest("El usuario ya tiene otro carrito activo");
                    }
                }

                // Actualizar propiedades
                carritoExistente.UsuarioId = request.UsuarioId;
                carritoExistente.Estado = request.Estado;
                carritoExistente.EstadoPago = request.EstadoPago;
                carritoExistente.MontoTotal = request.MontoTotal;
                carritoExistente.Saldo = request.Saldo;
                carritoExistente.DireccionEnvio = request.DireccionEnvio;

                var resultado = await repositorio.Update(id, carritoExistente);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el carrito");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/Carritos/2
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await repositorio.Delete(id);
            if (!resp)
            {
                return BadRequest("El carrito no se pudo borrar");
            }
            return Ok();
        }
    }

    // Clase auxiliar para el request de confirmar carrito
    public class ConfirmarCarritoRequest
    {
        public decimal MontoTotal { get; set; }
        public string DireccionEnvio { get; set; } = string.Empty;
    }

    // Clases DTO para crear y actualizar carritos
    public class CarritoCreateRequest
    {
        public int UsuarioId { get; set; }
        public string Estado { get; set; } = "Activo";
        public string EstadoPago { get; set; } = "Pendiente";
        public decimal MontoTotal { get; set; }
        public decimal Saldo { get; set; }
        public string DireccionEnvio { get; set; } = string.Empty;
    }

    public class CarritoUpdateRequest
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Estado { get; set; } = "Activo";
        public string EstadoPago { get; set; } = "Pendiente";
        public decimal MontoTotal { get; set; }
        public decimal Saldo { get; set; }
        public string DireccionEnvio { get; set; } = string.Empty;
    }
}
