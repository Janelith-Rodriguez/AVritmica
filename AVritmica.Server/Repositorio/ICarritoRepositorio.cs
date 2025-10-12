using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface ICarritoRepositorio
    {
        Task<List<Carrito>> Select();
        Task<Carrito?> SelectById(int id);
        Task<List<Carrito>> SelectByUsuario(int usuarioId);
        Task<List<Carrito>> SelectByEstado(string estado);
        Task<List<Carrito>> SelectByEstadoPago(string estadoPago);
        Task<Carrito?> SelectCarritoActivoByUsuario(int usuarioId);
        Task<bool> Existe(int id);
        Task<bool> ExisteCarritoActivo(int usuarioId);
        Task<int> Insert(Carrito entidad);
        Task<bool> Update(int id, Carrito entidad);
        Task<bool> Delete(int id);
        Task<bool> ActualizarEstado(int id, string estado);
        Task<bool> ActualizarEstadoPago(int id, string estadoPago);
        Task<bool> ConfirmarCarrito(int id, decimal montoTotal, string direccionEnvio);
        Task<bool> ActualizarMontoTotal(int id, decimal montoTotal);
    }
}
