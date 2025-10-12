using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface IProductoRepositorio
    {
        Task<List<Producto>> Select();
        Task<Producto?> SelectById(int id);
        Task<List<Producto>> SelectByCategoria(int categoriaId);
        Task<Producto?> SelectByNombre(string nombre);
        Task<List<Producto>> SelectByPrecioRange(decimal precioMin, decimal precioMax);
        Task<bool> Existe(int id);
        Task<bool> Existe(string nombre);
        Task<int> Insert(Producto entidad);
        Task<bool> Update(int id, Producto entidad);
        Task<bool> Delete(int id);
        Task<bool> ActualizarStock(int id, int cantidad);
    }
}
