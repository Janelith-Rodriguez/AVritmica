using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public class PagoRepositorio : Repositorio<Pago>, IPagoRepositorio
    {
        private readonly Context context;
        public PagoRepositorio(Context context) : base(context)
        {
            this.context = context;
        }

        //public async Task<List<Pago>> ObtenerPorUsuario(int usuarioId)
        //{
        //    return await context.Pagos
        //        .Where(p => p.UsuarioId == usuarioId)
        //        .ToListAsync();
        //}
    }
}
