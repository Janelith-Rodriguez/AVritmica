using AVritmica.BD.Data.Entity;

namespace AVritmica.Client.Servicios.Entidades
{
    public class CompraDetalleServicio : ICompraDetalleServicio
    {
        private readonly IHttpServicio http;
        private string url = "api/CompraDetalles";

        public CompraDetalleServicio(IHttpServicio http) => this.http = http;

        public async Task<HttpRespuesta<List<CompraDetalle>>> Get() => await http.Get<List<CompraDetalle>>(url);
        public async Task<HttpRespuesta<CompraDetalle>> Get(int id) => await http.Get<CompraDetalle>($"{url}/{id}");
        public async Task<HttpRespuesta<List<CompraDetalle>>> GetByCompra(int compraId) => await http.Get<List<CompraDetalle>>($"{url}/GetByCompra/{compraId}");
        public async Task<HttpRespuesta<List<CompraDetalle>>> GetByProducto(int productoId) => await http.Get<List<CompraDetalle>>($"{url}/GetByProducto/{productoId}");
        public async Task<HttpRespuesta<object>> Post(CompraDetalle entidad) => await http.Post(url, entidad);
        public async Task<HttpRespuesta<object>> Put(CompraDetalle entidad) => await http.Put($"{url}/{entidad.Id}", entidad);
        public async Task<HttpRespuesta<object>> Delete(int id) => await http.Delete($"{url}/{id}");
        public async Task<HttpRespuesta<decimal>> ObtenerTotalPorCompra(int compraId) => await http.Get<decimal>($"{url}/total-por-compra/{compraId}");
    }
}
