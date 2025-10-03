using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public class StockMovimientoRepositorio : Repositorio<StockMovimiento>, IStockMovimientoRepositorio
    {
        private readonly Context context;
        public StockMovimientoRepositorio(Context context) : base(context)
        {
            this.context = context;
        }
    }
}
