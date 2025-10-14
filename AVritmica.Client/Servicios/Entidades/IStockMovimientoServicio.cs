using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;

namespace AVritmica.Client.Servicios.Entidades
{
    public interface IStockMovimientoServicio
    {
        Task<HttpRespuesta<List<StockMovimiento>>> Get();
        Task<HttpRespuesta<StockMovimiento>> Get(int id);
        Task<HttpRespuesta<List<StockMovimiento>>> GetByProducto(int productoId);
        Task<HttpRespuesta<List<StockMovimiento>>> GetByTipoMovimiento(string tipoMovimiento);
        Task<HttpRespuesta<object>> Post(StockMovimiento entidad);
        Task<HttpRespuesta<object>> Put(StockMovimiento entidad);
        Task<HttpRespuesta<object>> Delete(int id);
        Task<HttpRespuesta<object>> RegistrarMovimiento(int productoId, string tipoMovimiento, int cantidad, string descripcion);
        Task<HttpRespuesta<int>> ObtenerStockActual(int productoId);
    }
}
