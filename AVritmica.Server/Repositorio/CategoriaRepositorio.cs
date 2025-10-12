using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly Context _context;

        public CategoriaRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> Select()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria?> SelectById(int id)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Categoria?> SelectByNombre(string nombre)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(x => x.Nombre == nombre);
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Categorias
                .AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Existe(string nombre)
        {
            return await _context.Categorias
                .AnyAsync(x => x.Nombre == nombre);
        }

        public async Task<int> Insert(Categoria entidad)
        {
            await _context.Categorias.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad.Id;
        }

        public async Task<bool> Update(int id, Categoria entidad)
        {
            var categoriaExistente = await _context.Categorias
                .FirstOrDefaultAsync(x => x.Id == id);

            if (categoriaExistente == null)
                return false;

            categoriaExistente.Nombre = entidad.Nombre;
            categoriaExistente.Descripcion = entidad.Descripcion;

            _context.Categorias.Update(categoriaExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(x => x.Id == id);

            if (categoria == null)
                return false;

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}