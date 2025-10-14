using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;

namespace AVritmica.Client.Servicios.Entidades
{
    public interface IPagoServicio
    {
        Task<HttpRespuesta<List<Pago>>> Get();
        Task<HttpRespuesta<Pago>> Get(int id);
        Task<HttpRespuesta<List<Pago>>> GetByCarrito(int carritoId);
        Task<HttpRespuesta<List<Pago>>> GetByEstado(string estadoPago);
        Task<HttpRespuesta<object>> Post(Pago entidad);
        Task<HttpRespuesta<object>> Put(Pago entidad);
        Task<HttpRespuesta<object>> Delete(int id);
        Task<HttpRespuesta<object>> ProcesarPago(int carritoId, string metodoPago, decimal montoPagado);
        Task<HttpRespuesta<decimal>> ObtenerTotalPagadoPorCarrito(int carritoId);
    }
}
