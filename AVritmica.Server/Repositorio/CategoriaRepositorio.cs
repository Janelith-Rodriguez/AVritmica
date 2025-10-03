using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AVritmica.Server.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly Context context;
        public CategoriaRepositorio(Context context) : base(context)
        {
            this.context = context;
        }
        public async Task<List<Categoria>> BuscarPorNombre(string nombre)
        {
            return await context.Categorias
                .Where(c => c.Nombre.Contains(nombre))
                .ToListAsync();
        }
    }
}
