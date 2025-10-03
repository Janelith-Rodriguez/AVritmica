using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public class CompraDetalleRepositorio : Repositorio<CompraDetalle>, ICompraDetalleRepositorio
    {
        private readonly Context context;
        public CompraDetalleRepositorio(Context context) : base(context)
        {
            this.context = context;
        }
    }
}
