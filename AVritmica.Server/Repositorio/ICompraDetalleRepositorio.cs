using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface ICompraDetalleRepositorio
    {
        Task<List<CompraDetalle>> Select();
        Task<CompraDetalle?> SelectById(int id);
        Task<List<CompraDetalle>> SelectByCompra(int compraId);
        Task<List<CompraDetalle>> SelectByProducto(int productoId);
        Task<CompraDetalle?> SelectByCompraAndProducto(int compraId, int productoId);
        Task<bool> Existe(int id);
        Task<bool> Existe(int compraId, int productoId);
        Task<int> Insert(CompraDetalle entidad);
        Task<bool> Update(int id, CompraDetalle entidad);
        Task<bool> Delete(int id);
        Task<bool> DeleteByCompraAndProducto(int compraId, int productoId);
        Task<bool> ActualizarCantidad(int id, int cantidad);
        Task<bool> ActualizarPrecios(int id, decimal precioCompra, decimal precioVentaActualizado);
        Task<decimal> ObtenerTotalPorCompra(int compraId);
        Task<int> ObtenerCantidadTotalPorProducto(int productoId);
        Task<List<CompraDetalle>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin);
    }
}
