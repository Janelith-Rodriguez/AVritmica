using AVritmica.BD.Data.Entity;

namespace AVritmica.Client.Servicios.Entidades
{
    public class CarritoServicio : ICarritoServicio
    {
        private readonly IHttpServicio http;
        private string url = "api/Carritos";

        public CarritoServicio(IHttpServicio http) => this.http = http;

        public async Task<HttpRespuesta<List<Carrito>>> Get() => await http.Get<List<Carrito>>(url);
        public async Task<HttpRespuesta<Carrito>> Get(int id) => await http.Get<Carrito>($"{url}/{id}");
        public async Task<HttpRespuesta<List<Carrito>>> GetByUsuario(int usuarioId) => await http.Get<List<Carrito>>($"{url}/GetByUsuario/{usuarioId}");
        public async Task<HttpRespuesta<List<Carrito>>> GetByEstado(string estado) => await http.Get<List<Carrito>>($"{url}/GetByEstado/{estado}");
        public async Task<HttpRespuesta<Carrito>> GetCarritoActivo(int usuarioId) => await http.Get<Carrito>($"{url}/GetCarritoActivo/{usuarioId}");
        public async Task<HttpRespuesta<object>> Post(Carrito entidad) => await http.Post(url, entidad);
        public async Task<HttpRespuesta<object>> Put(Carrito entidad) => await http.Put($"{url}/{entidad.Id}", entidad);
        public async Task<HttpRespuesta<object>> Delete(int id) => await http.Delete($"{url}/{id}");
        public async Task<HttpRespuesta<object>> ActualizarEstado(int id, string estado) => await http.Post($"{url}/actualizar-estado/{id}", estado);
        public async Task<HttpRespuesta<object>> ConfirmarCarrito(int id, decimal montoTotal, string direccionEnvio) => await http.Post($"{url}/confirmar-carrito/{id}", new { montoTotal, direccionEnvio });
    }
}
