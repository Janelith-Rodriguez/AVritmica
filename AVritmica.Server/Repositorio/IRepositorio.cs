using AVritmica.BD.Data;

namespace AVritmica.Server.Repositorio
{
    public interface IRepositorio<E> where E : class, IEntityBase
    {
        Task<bool> Delete(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(E entidad);
        Task<List<E>> Select();
        Task<E?> SelectById(int id);  // Cambiado a nullable
        Task<bool> Update(int id, E entidad);
    }
}