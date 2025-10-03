using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AVritmica.Server.Repositorio
{
    public class CompraRepositorio : Repositorio<Compra>, ICompraRepositorio
    {
        private readonly Context context;
        public CompraRepositorio(Context context) : base(context)
        {
            this.context = context;
        }

        //public async Task<List<Compra>> ObtenerPorUsuario(int usuarioId)
        //{
        //    return await context.Compras
        //        .Where(c => c.UsuarioId == usuarioId)
        //        .ToListAsync();
        //}
    }
}
