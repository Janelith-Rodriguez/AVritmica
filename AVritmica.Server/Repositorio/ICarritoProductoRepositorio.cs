using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface ICarritoProductoRepositorio
    {
        Task<List<CarritoProducto>> Select();
        Task<CarritoProducto?> SelectById(int id);
        Task<List<CarritoProducto>> SelectByCarrito(int carritoId);
        Task<List<CarritoProducto>> SelectByProducto(int productoId);
        Task<CarritoProducto?> SelectByCarritoAndProducto(int carritoId, int productoId);
        Task<bool> Existe(int id);
        Task<bool> Existe(int carritoId, int productoId);
        Task<int> Insert(CarritoProducto entidad);
        Task<bool> Update(int id, CarritoProducto entidad);
        Task<bool> Delete(int id);
        Task<bool> DeleteByCarritoAndProducto(int carritoId, int productoId);
        Task<bool> ActualizarCantidad(int id, int cantidad);
        Task<bool> ActualizarPrecioUnitario(int id, decimal precioUnitario);
        Task<int> ObtenerCantidadTotalEnCarritos(int productoId);
    }
}