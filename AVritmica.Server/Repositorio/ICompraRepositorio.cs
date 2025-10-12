using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface ICompraRepositorio
    {
        Task<List<Compra>> Select();
        Task<Compra?> SelectById(int id);
        Task<List<Compra>> SelectByFecha(DateTime fecha);
        Task<List<Compra>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<bool> Existe(int id);
        Task<int> Insert(Compra entidad);
        Task<bool> Update(int id, Compra entidad);
        Task<bool> Delete(int id);
        Task<decimal> ObtenerTotalCompra(int id);
        Task<int> ObtenerCantidadTotalProductos(int id);
    }
}
