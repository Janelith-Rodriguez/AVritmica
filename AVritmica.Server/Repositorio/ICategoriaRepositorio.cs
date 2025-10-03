using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface ICategoriaRepositorio : IRepositorio<Categoria>
    {
        Task<List<Categoria>> BuscarPorNombre(string nombre);
    }
}
