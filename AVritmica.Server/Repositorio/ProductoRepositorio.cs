using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AVritmica.Server.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly Context context;
        public ProductoRepositorio(Context context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Producto>> ObtenerPorCategoria(int categoriaId)
        {
            return await context.Productos
                .Where(p => p.CategoriaId == categoriaId)
                .ToListAsync();
        }
    }
}
