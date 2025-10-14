using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;

namespace AVritmica.Client.Servicios.Entidades
{
    public interface ICarritoProductoServicio
    {
        Task<HttpRespuesta<List<CarritoProducto>>> Get();
        Task<HttpRespuesta<CarritoProducto>> Get(int id);
        Task<HttpRespuesta<List<CarritoProducto>>> GetByCarrito(int carritoId);
        Task<HttpRespuesta<CarritoProducto>> GetByCarritoAndProducto(int carritoId, int productoId);
        Task<HttpRespuesta<object>> Post(CarritoProducto entidad);
        Task<HttpRespuesta<object>> Put(CarritoProducto entidad);
        Task<HttpRespuesta<object>> Delete(int id);
        Task<HttpRespuesta<object>> ActualizarCantidad(int id, int cantidad);
        Task<HttpRespuesta<object>> DeleteByCarritoAndProducto(int carritoId, int productoId);
    }
}
