using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;

namespace AVritmica.Client.Servicios.Entidades
{
    public interface ICompraServicio
    {
        Task<HttpRespuesta<List<Compra>>> Get();
        Task<HttpRespuesta<Compra>> Get(int id);
        Task<HttpRespuesta<List<Compra>>> GetByFecha(DateTime fecha);
        Task<HttpRespuesta<List<Compra>>> GetByRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<HttpRespuesta<object>> Post(Compra entidad);
        Task<HttpRespuesta<object>> Put(Compra entidad);
        Task<HttpRespuesta<object>> Delete(int id);
        Task<HttpRespuesta<decimal>> ObtenerTotalCompra(int id);
        Task<HttpRespuesta<int>> ObtenerCantidadTotalProductos(int id);
    }
}
