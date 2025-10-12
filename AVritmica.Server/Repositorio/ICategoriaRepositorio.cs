using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface ICategoriaRepositorio
    {
        Task<List<Categoria>> Select();
        Task<Categoria?> SelectById(int id);
        Task<Categoria?> SelectByNombre(string nombre);
        Task<bool> Existe(int id);
        Task<bool> Existe(string nombre);
        Task<int> Insert(Categoria entidad);
        Task<bool> Update(int id, Categoria entidad);
        Task<bool> Delete(int id);
    }
}
