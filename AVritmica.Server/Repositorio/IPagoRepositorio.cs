using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface IPagoRepositorio
    {
        Task<List<Pago>> Select();
        Task<Pago?> SelectById(int id);
        Task<List<Pago>> SelectByCarrito(int carritoId);
        Task<List<Pago>> SelectByEstado(string estadoPago);
        Task<List<Pago>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<List<Pago>> SelectByMetodoPago(string metodoPago);
        Task<decimal> ObtenerTotalPagadoPorCarrito(int carritoId);
        Task<bool> Existe(int id);
        Task<int> Insert(Pago entidad);
        Task<bool> Update(int id, Pago entidad);
        Task<bool> Delete(int id);
        Task<bool> ActualizarEstadoPago(int id, string estadoPago);
        Task<bool> ProcesarPago(int carritoId, string metodoPago, decimal montoPagado);
        Task<decimal> ObtenerSaldoPendientePorCarrito(int carritoId);
    }
}