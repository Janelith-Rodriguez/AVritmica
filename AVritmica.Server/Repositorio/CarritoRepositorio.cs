using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AVritmica.Server.Repositorio
{
    public class CarritoRepositorio : Repositorio<Carrito>, ICarritoRepositorio
    {
        private readonly Context context;

        public CarritoRepositorio(Context context) : base(context)
        {
            this.context = context;
        }

        public async Task<Carrito> ObtenerPorUsuario(int usuarioId)
        {
            return await context.Carritos
                                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }
    }
}
