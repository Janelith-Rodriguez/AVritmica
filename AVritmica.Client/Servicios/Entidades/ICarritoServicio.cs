using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;

namespace AVritmica.Client.Servicios.Entidades
{
    public interface ICarritoServicio
    {
        Task<HttpRespuesta<List<Carrito>>> Get();
        Task<HttpRespuesta<Carrito>> Get(int id);
        Task<HttpRespuesta<List<Carrito>>> GetByUsuario(int usuarioId);
        Task<HttpRespuesta<List<Carrito>>> GetByEstado(string estado);
        Task<HttpRespuesta<Carrito>> GetCarritoActivo(int usuarioId);
        Task<HttpRespuesta<object>> Post(Carrito entidad);
        Task<HttpRespuesta<object>> Put(Carrito entidad);
        Task<HttpRespuesta<object>> Delete(int id);
        Task<HttpRespuesta<object>> ActualizarEstado(int id, string estado);
        Task<HttpRespuesta<object>> ConfirmarCarrito(int id, decimal montoTotal, string direccionEnvio);
    }
}
