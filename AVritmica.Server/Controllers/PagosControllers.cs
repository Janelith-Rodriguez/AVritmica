using Microsoft.AspNetCore.Mvc;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;
using System.ComponentModel.DataAnnotations;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/Pagos")]
    public class PagosController : ControllerBase
    {
        private readonly IPagoRepositorio repositorio;

        public PagosController(IPagoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/Pagos
        public async Task<ActionResult<List<Pago>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener un pago por ID
        /// </summary>
        /// <param name="id">Id del pago</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] // api/Pagos/2
        public async Task<ActionResult<Pago>> Get(int id)
        {
            Pago? pago = await repositorio.SelectById(id);
            if (pago == null)
            {
                return NotFound();
            }
            return pago;
        }

        [HttpGet("GetByCarrito/{carritoId:int}")] // api/Pagos/GetByCarrito/1
        public async Task<ActionResult<List<Pago>>> GetByCarrito(int carritoId)
        {
            var pagos = await repositorio.SelectByCarrito(carritoId);
            return pagos;
        }

        [HttpGet("GetByEstado/{estadoPago}")] // api/Pagos/GetByEstado/Completado
        public async Task<ActionResult<List<Pago>>> GetByEstado(string estadoPago)
        {
            var pagos = await repositorio.SelectByEstado(estadoPago);
            return pagos;
        }

        [HttpGet("GetByRangoFechas")] // api/Pagos/GetByRangoFechas?fechaInicio=2024-01-01&fechaFin=2024-01-31
        public async Task<ActionResult<List<Pago>>> GetByRangoFechas([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            // Validar que la fecha de inicio no sea mayor que la fecha fin
            if (fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");
            }

            var pagos = await repositorio.SelectByRangoFechas(fechaInicio, fechaFin);
            return pagos;
        }

        [HttpGet("GetByMetodoPago/{metodoPago}")] // api/Pagos/GetByMetodoPago/Tarjeta
        public async Task<ActionResult<List<Pago>>> GetByMetodoPago(string metodoPago)
        {
            var pagos = await repositorio.SelectByMetodoPago(metodoPago);
            return pagos;
        }

        [HttpGet("existe/{id:int}")] // api/Pagos/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("total-pagado-carrito/{carritoId:int}")] // api/Pagos/total-pagado-carrito/1
        public async Task<ActionResult<decimal>> ObtenerTotalPagadoPorCarrito(int carritoId)
        {
            var total = await repositorio.ObtenerTotalPagadoPorCarrito(carritoId);
            return total;
        }

        [HttpGet("saldo-pendiente-carrito/{carritoId:int}")] // api/Pagos/saldo-pendiente-carrito/1
        public async Task<ActionResult<decimal>> ObtenerSaldoPendientePorCarrito(int carritoId)
        {
            var saldo = await repositorio.ObtenerSaldoPendientePorCarrito(carritoId);
            return saldo;
        }

        [HttpPost("actualizar-estado/{id:int}")] // api/Pagos/actualizar-estado/2
        public async Task<ActionResult> ActualizarEstadoPago(int id, [FromBody] string estadoPago)
        {
            try
            {
                var resultado = await repositorio.ActualizarEstadoPago(id, estadoPago);
                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el estado del pago");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("procesar-pago")] // api/Pagos/procesar-pago
        public async Task<ActionResult<int>> ProcesarPago([FromBody] ProcesarPagoRequest request)
        {
            try
            {
                // Validar que el monto sea positivo
                if (request.MontoPagado <= 0)
                {
                    return BadRequest("El monto pagado debe ser mayor a cero");
                }

                // Validar que el método de pago no esté vacío
                if (string.IsNullOrWhiteSpace(request.MetodoPago))
                {
                    return BadRequest("El método de pago es obligatorio");
                }

                var resultado = await repositorio.ProcesarPago(request.CarritoId, request.MetodoPago, request.MontoPagado);
                if (!resultado)
                {
                    return BadRequest("No se pudo procesar el pago. Verifique el monto o el carrito");
                }

                // Obtener el ID del último pago insertado
                var pagosDelCarrito = await repositorio.SelectByCarrito(request.CarritoId);
                var ultimoPago = pagosDelCarrito.OrderByDescending(p => p.FechaPago).FirstOrDefault();

                return ultimoPago?.Id ?? 0;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Método POST original mantenido para compatibilidad
        [HttpPost]
        public async Task<ActionResult<int>> Post(Pago entidad)
        {
            try
            {
                // Validar que el monto sea positivo
                if (entidad.MontoPagado <= 0)
                {
                    return BadRequest("El monto pagado debe ser mayor a cero");
                }

                // Validar que el método de pago no esté vacío
                if (string.IsNullOrWhiteSpace(entidad.MetodoPago))
                {
                    return BadRequest("El método de pago es obligatorio");
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
        public async Task<ActionResult<int>> CrearPago([FromBody] PagoCreateRequest request)
        {
            try
            {
                // Validar que el monto sea positivo
                if (request.MontoPagado <= 0)
                {
                    return BadRequest("El monto pagado debe ser mayor a cero");
                }

                // Validar que el método de pago no esté vacío
                if (string.IsNullOrWhiteSpace(request.MetodoPago))
                {
                    return BadRequest("El método de pago es obligatorio");
                }

                var pago = new Pago
                {
                    CarritoId = request.CarritoId,
                    MetodoPago = request.MetodoPago,
                    MontoPagado = request.MontoPagado,
                    EstadoPago = request.EstadoPago,
                    Saldo = request.Saldo,
                    FechaPago = DateTime.UtcNow
                };

                return await repositorio.Insert(pago);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        // Método PUT original mantenido para compatibilidad
        [HttpPut("{id:int}")] // api/Pagos/2
        public async Task<ActionResult> Put(int id, [FromBody] Pago entidad)
        {
            try
            {
                if (id != entidad.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                // Validar que el monto sea positivo
                if (entidad.MontoPagado <= 0)
                {
                    return BadRequest("El monto pagado debe ser mayor a cero");
                }

                // Validar que el método de pago no esté vacío
                if (string.IsNullOrWhiteSpace(entidad.MetodoPago))
                {
                    return BadRequest("El método de pago es obligatorio");
                }

                var resultado = await repositorio.Update(id, entidad);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el pago");
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
        public async Task<ActionResult> ActualizarPago(int id, [FromBody] PagoUpdateRequest request)
        {
            try
            {
                if (id != request.Id)
                {
                    return BadRequest("Datos Incorrectos");
                }

                var pagoExistente = await repositorio.SelectById(id);
                if (pagoExistente == null)
                {
                    return NotFound();
                }

                // Validar que el monto sea positivo
                if (request.MontoPagado <= 0)
                {
                    return BadRequest("El monto pagado debe ser mayor a cero");
                }

                // Validar que el método de pago no esté vacío
                if (string.IsNullOrWhiteSpace(request.MetodoPago))
                {
                    return BadRequest("El método de pago es obligatorio");
                }

                // Actualizar propiedades
                pagoExistente.CarritoId = request.CarritoId;
                pagoExistente.MetodoPago = request.MetodoPago;
                pagoExistente.MontoPagado = request.MontoPagado;
                pagoExistente.EstadoPago = request.EstadoPago;
                pagoExistente.Saldo = request.Saldo;
                // No actualizamos FechaPago para mantener la fecha original

                var resultado = await repositorio.Update(id, pagoExistente);

                if (!resultado)
                {
                    return BadRequest("No se pudo actualizar el pago");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/Pagos/2
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await repositorio.Delete(id);
            if (!resp)
            {
                return BadRequest("El pago no se pudo borrar");
            }
            return Ok();
        }
    }

    // Clases auxiliares para requests
    public class ProcesarPagoRequest
    {
        public int CarritoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string MetodoPago { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal MontoPagado { get; set; }
    }

    // Clases DTO para crear y actualizar pagos
    public class PagoCreateRequest
    {
        public int CarritoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string MetodoPago { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal MontoPagado { get; set; }

        [Required]
        [MaxLength(20)]
        public string EstadoPago { get; set; } = "Completado";

        public decimal Saldo { get; set; }
    }

    public class PagoUpdateRequest
    {
        public int Id { get; set; }
        public int CarritoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string MetodoPago { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal MontoPagado { get; set; }

        [Required]
        [MaxLength(20)]
        public string EstadoPago { get; set; } = "Completado";

        public decimal Saldo { get; set; }
    }
}
