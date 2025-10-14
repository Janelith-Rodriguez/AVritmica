using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;

namespace AVritmica.Client.Servicios.Entidades
{
    public interface ICompraDetalleServicio
    {
        Task<HttpRespuesta<List<CompraDetalle>>> Get();
        Task<HttpRespuesta<CompraDetalle>> Get(int id);
        Task<HttpRespuesta<List<CompraDetalle>>> GetByCompra(int compraId);
        Task<HttpRespuesta<List<CompraDetalle>>> GetByProducto(int productoId);
        Task<HttpRespuesta<object>> Post(CompraDetalle entidad);
        Task<HttpRespuesta<object>> Put(CompraDetalle entidad);
        Task<HttpRespuesta<object>> Delete(int id);
        Task<HttpRespuesta<decimal>> ObtenerTotalPorCompra(int compraId);
    }
}
