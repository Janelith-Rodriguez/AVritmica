using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        Task<List<Producto>> ObtenerPorCategoria(int categoriaId);
    }
}
