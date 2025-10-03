using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public class CarritoProductoRepositorio : Repositorio<CarritoProducto>, ICarritoProductoRepositorio
    {
        private readonly Context context;
        public CarritoProductoRepositorio(Context context) : base(context)
        {
            this.context = context;
        }
    }
}
