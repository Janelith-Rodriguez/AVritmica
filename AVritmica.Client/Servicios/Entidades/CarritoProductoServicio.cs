using AVritmica.BD.Data.Entity;

namespace AVritmica.Client.Servicios.Entidades
{
    public class CarritoProductoServicio : ICarritoProductoServicio
    {
        private readonly IHttpServicio http;
        private string url = "api/CarritoProductos";

        public CarritoProductoServicio(IHttpServicio http) => this.http = http;

        public async Task<HttpRespuesta<List<CarritoProducto>>> Get() => await http.Get<List<CarritoProducto>>(url);
        public async Task<HttpRespuesta<CarritoProducto>> Get(int id) => await http.Get<CarritoProducto>($"{url}/{id}");
        public async Task<HttpRespuesta<List<CarritoProducto>>> GetByCarrito(int carritoId) => await http.Get<List<CarritoProducto>>($"{url}/GetByCarrito/{carritoId}");
        public async Task<HttpRespuesta<CarritoProducto>> GetByCarritoAndProducto(int carritoId, int productoId) => await http.Get<CarritoProducto>($"{url}/GetByCarritoAndProducto?carritoId={carritoId}&productoId={productoId}");
        public async Task<HttpRespuesta<object>> Post(CarritoProducto entidad) => await http.Post(url, entidad);
        public async Task<HttpRespuesta<object>> Put(CarritoProducto entidad) => await http.Put($"{url}/{entidad.Id}", entidad);
        public async Task<HttpRespuesta<object>> Delete(int id) => await http.Delete($"{url}/{id}");
        public async Task<HttpRespuesta<object>> ActualizarCantidad(int id, int cantidad) => await http.Post($"{url}/actualizar-cantidad/{id}", cantidad);
        public async Task<HttpRespuesta<object>> DeleteByCarritoAndProducto(int carritoId, int productoId) => await http.Delete($"{url}/DeleteByCarritoAndProducto?carritoId={carritoId}&productoId={productoId}");
    }
}
