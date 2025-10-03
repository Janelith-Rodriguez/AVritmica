using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface ICarritoRepositorio : IRepositorio<Carrito>
    {
        Task<Carrito> ObtenerPorUsuario(int usuarioId);
    }
}
