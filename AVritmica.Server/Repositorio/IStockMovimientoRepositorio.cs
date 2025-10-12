using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface IStockMovimientoRepositorio
    {
        Task<List<StockMovimiento>> Select();
        Task<StockMovimiento?> SelectById(int id);
        Task<List<StockMovimiento>> SelectByProducto(int productoId);
        Task<List<StockMovimiento>> SelectByTipoMovimiento(string tipoMovimiento);
        Task<List<StockMovimiento>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<List<StockMovimiento>> SelectByProductoAndRangoFechas(int productoId, DateTime fechaInicio, DateTime fechaFin);
        Task<bool> Existe(int id);
        Task<int> Insert(StockMovimiento entidad);
        Task<bool> Update(int id, StockMovimiento entidad);
        Task<bool> Delete(int id);
        Task<int> ObtenerStockActual(int productoId);
        Task<int> ObtenerEntradasTotales(int productoId);
        Task<int> ObtenerSalidasTotales(int productoId);
        Task<bool> RegistrarMovimiento(int productoId, string tipoMovimiento, int cantidad, string descripcion = "");
    }
}
