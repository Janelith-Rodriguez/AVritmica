using Microsoft.AspNetCore.Mvc;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.Controllers
{
    [ApiController]
    [Route("api/Compras")]
    public class ComprasController : ControllerBase
    {
        private readonly ICompraRepositorio repositorio;

        public ComprasController(ICompraRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]    // api/Compras
        public async Task<ActionResult<List<Compra>>> Get()
        {
            return await repositorio.Select();
        }

        /// <summary>
        /// Endpoint para obtener una compra por ID
        /// </summary>
        /// <param name="id">Id de la compra</param>
        /// <returns></returns>
        [HttpGet("{id:int}")] // api/Compras/2
        public async Task<ActionResult<Compra>> Get(int id)
        {
            Compra? compra = await repositorio.SelectById(id);
            if (compra == null)
            {
                return NotFound();
            }
            return compra;
        }

        [HttpGet("GetByFecha/{fecha:datetime}")] // api/Compras/GetByFecha/2024-01-15
        public async Task<ActionResult<List<Compra>>> GetByFecha(DateTime fecha)
        {
            var compras = await repositorio.SelectByFecha(fecha);
            return compras;
        }

        [HttpGet("GetByRangoFechas")] // api/Compras/GetByRangoFechas?fechaInicio=2024-01-01&fechaFin=2024-01-31
        public async Task<ActionResult<List<Compra>>> GetByRangoFechas([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            // Validar que la fecha de inicio no sea mayor que la fecha fin
            if (fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin");
            }

            var compras = await repositorio.SelectByRangoFechas(fechaInicio, fechaFin);
            return compras;
        }

        [HttpGet("existe/{id:int}")] // api/Compras/existe/2
        public async Task<ActionResult<bool>> Existe(int id)
        {
            return await repositorio.Existe(id);
        }

        [HttpGet("total/{id:int}")] // api/Compras/total/2
        public async Task<ActionResult<decimal>> ObtenerTotalCompra(int id)
        {
            var total = await repositorio.ObtenerTotalCompra(id);
            return total;
        }

        [HttpGet("cantidad-productos/{id:int}")] // api/Compras/cantidad-productos/2
        public async Task<ActionResult<int>> ObtenerCantidadTotalProductos(int id)
        {
            var cantidad = await repositorio.ObtenerCantidadTotalProductos(id);
            return cantidad;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Compra entidad)
        {
            try
            {
                // Asegurar que la fecha de compra sea la actual si no se especifica
                if (entidad.FechaCompra == default)
                {
                    entidad.FechaCompra = DateTime.UtcNow;
                }

                return await repositorio.Insert(entidad);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")] // api/Compras/2
        public async Task<ActionResult> Put(int id, [FromBody] Compra entidad)
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
                    return BadRequest("No se pudo actualizar la compra");
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")] // api/Compras/2
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await repositorio.Delete(id);
            if (!resp)
            {
                return BadRequest("La compra no se pudo borrar");
            }
            return Ok();
        }

        /// <summary>
        /// Endpoint para crear una compra con sus detalles
        /// </summary>
        [HttpPost("crear-con-detalles")]
        public async Task<ActionResult<int>> CrearCompraConDetalles([FromBody] CrearCompraRequest request)
        {
            try
            {
                var compra = new Compra
                {
                    FechaCompra = request.FechaCompra != default ? request.FechaCompra : DateTime.UtcNow,
                    Descripcion = request.Descripcion,
                    CompraDetalles = request.Detalles
                };

                return await repositorio.Insert(compra);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }

    // Clases auxiliares para requests
    public class CrearCompraRequest
    {
        public DateTime FechaCompra { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public List<CompraDetalle> Detalles { get; set; } = new List<CompraDetalle>();
    }
}