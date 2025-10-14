using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;
using AVritmica.Client.Servicios.Entidades;

namespace AVritmica.Client.Servicios.Entidades
{
    public class StockMovimientoServicio : IStockMovimientoServicio
    {
        private readonly IHttpServicio http;
        private string url = "api/StockMovimientos";

        public StockMovimientoServicio(IHttpServicio http) => this.http = http;

        public async Task<HttpRespuesta<List<StockMovimiento>>> Get() => await http.Get<List<StockMovimiento>>(url);
        public async Task<HttpRespuesta<StockMovimiento>> Get(int id) => await http.Get<StockMovimiento>($"{url}/{id}");
        public async Task<HttpRespuesta<List<StockMovimiento>>> GetByProducto(int productoId) => await http.Get<List<StockMovimiento>>($"{url}/GetByProducto/{productoId}");
        public async Task<HttpRespuesta<List<StockMovimiento>>> GetByTipoMovimiento(string tipoMovimiento) => await http.Get<List<StockMovimiento>>($"{url}/GetByTipoMovimiento/{tipoMovimiento}");
        public async Task<HttpRespuesta<object>> Post(StockMovimiento entidad) => await http.Post(url, entidad);
        public async Task<HttpRespuesta<object>> Put(StockMovimiento entidad) => await http.Put($"{url}/{entidad.Id}", entidad);
        public async Task<HttpRespuesta<object>> Delete(int id) => await http.Delete($"{url}/{id}");
        public async Task<HttpRespuesta<object>> RegistrarMovimiento(int productoId, string tipoMovimiento, int cantidad, string descripcion) => await http.Post($"{url}/registrar-movimiento", new { productoId, tipoMovimiento, cantidad, descripcion });
        public async Task<HttpRespuesta<int>> ObtenerStockActual(int productoId) => await http.Get<int>($"{url}/stock-actual/{productoId}");
    }
}
