using AVritmica.BD.Data.Entity;

namespace AVritmica.Client.Servicios.Entidades
{
    public class PagoServicio : IPagoServicio
    {
        private readonly IHttpServicio http;
        private string url = "api/Pagos";

        public PagoServicio(IHttpServicio http) => this.http = http;

        public async Task<HttpRespuesta<List<Pago>>> Get() => await http.Get<List<Pago>>(url);
        public async Task<HttpRespuesta<Pago>> Get(int id) => await http.Get<Pago>($"{url}/{id}");
        public async Task<HttpRespuesta<List<Pago>>> GetByCarrito(int carritoId) => await http.Get<List<Pago>>($"{url}/GetByCarrito/{carritoId}");
        public async Task<HttpRespuesta<List<Pago>>> GetByEstado(string estadoPago) => await http.Get<List<Pago>>($"{url}/GetByEstado/{estadoPago}");
        public async Task<HttpRespuesta<object>> Post(Pago entidad) => await http.Post(url, entidad);
        public async Task<HttpRespuesta<object>> Put(Pago entidad) => await http.Put($"{url}/{entidad.Id}", entidad);
        public async Task<HttpRespuesta<object>> Delete(int id) => await http.Delete($"{url}/{id}");
        public async Task<HttpRespuesta<object>> ProcesarPago(int carritoId, string metodoPago, decimal montoPagado) => await http.Post($"{url}/procesar-pago", new { carritoId, metodoPago, montoPagado });
        public async Task<HttpRespuesta<decimal>> ObtenerTotalPagadoPorCarrito(int carritoId) => await http.Get<decimal>($"{url}/total-pagado-carrito/{carritoId}");
    }
}
