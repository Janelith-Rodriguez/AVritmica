using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;
using AVritmica.Shared.DTO;
using System.Text;
using System.Text.Json;

namespace AVritmica.Client.Servicios
{
    public interface IProductoService
    {
        Task<HttpRespuesta<List<Producto>>> GetProductos();
        Task<HttpRespuesta<Producto>> GetProducto(int id);
        Task<HttpRespuesta<List<Producto>>> GetProductosPorCategoria(int categoriaId);
        Task<HttpRespuesta<List<Producto>>> GetProductosPorPrecioRange(decimal precioMin, decimal precioMax);
        Task<HttpRespuesta<int>> CreateProducto(CrearProductoDTO producto);
        Task<HttpRespuesta<bool>> UpdateProducto(int id, Producto producto);
        Task<HttpRespuesta<bool>> DeleteProducto(int id);
        Task<HttpRespuesta<bool>> ActualizarStock(int id, int cantidad);
        Task<HttpRespuesta<bool>> ExisteNombre(string nombre);
    }
}
