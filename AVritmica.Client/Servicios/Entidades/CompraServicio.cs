using AVritmica.BD.Data.Entity;

namespace AVritmica.Client.Servicios.Entidades
{
    public class CompraServicio : ICompraServicio
    {
        private readonly IHttpServicio http;
        private string url = "api/Compras";

        public CompraServicio(IHttpServicio http) => this.http = http;

        public async Task<HttpRespuesta<List<Compra>>> Get() => await http.Get<List<Compra>>(url);
        public async Task<HttpRespuesta<Compra>> Get(int id) => await http.Get<Compra>($"{url}/{id}");
        public async Task<HttpRespuesta<List<Compra>>> GetByFecha(DateTime fecha) => await http.Get<List<Compra>>($"{url}/GetByFecha/{fecha:yyyy-MM-dd}");
        public async Task<HttpRespuesta<List<Compra>>> GetByRangoFechas(DateTime fechaInicio, DateTime fechaFin) => await http.Get<List<Compra>>($"{url}/GetByRangoFechas?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        public async Task<HttpRespuesta<object>> Post(Compra entidad) => await http.Post(url, entidad);
        public async Task<HttpRespuesta<object>> Put(Compra entidad) => await http.Put($"{url}/{entidad.Id}", entidad);
        public async Task<HttpRespuesta<object>> Delete(int id) => await http.Delete($"{url}/{id}");
        public async Task<HttpRespuesta<decimal>> ObtenerTotalCompra(int id) => await http.Get<decimal>($"{url}/total/{id}");
        public async Task<HttpRespuesta<int>> ObtenerCantidadTotalProductos(int id) => await http.Get<int>($"{url}/cantidad-productos/{id}");
    }
}
