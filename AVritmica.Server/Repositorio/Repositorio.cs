using AVritmica.BD.Data;
using Microsoft.EntityFrameworkCore;

namespace AVritmica.Server.Repositorio
{
    public class Repositorio<E> : IRepositorio<E>
                 where E : class, IEntityBase
    {
        protected readonly Context context;

        public Repositorio(Context context)
        {
            this.context = context;
        }

        public async Task<bool> Existe(int id)
        {
            var existe = await context.Set<E>()
                             .AnyAsync(x => x.Id == id);
            return existe;
        }

        public async Task<List<E>> Select()
        {
            return await context.Set<E>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<E?> SelectById(int id)
        {
            E? entidad = await context.Set<E>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return entidad;
        }

        public async Task<int> Insert(E entidad)
        {
            try
            {
                await context.Set<E>().AddAsync(entidad);
                await context.SaveChangesAsync();
                return entidad.Id;
            }
            catch (Exception e)
            {
                throw new Exception($"Error al insertar la entidad: {e.Message}", e);
            }
        }

        public async Task<bool> Update(int id, E entidad)
        {
            if (id != entidad.Id)
            {
                return false;
            }

            var entidadExistente = await context.Set<E>()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidadExistente == null)
            {
                return false;
            }

            try
            {
                context.Entry(entidadExistente).CurrentValues.SetValues(entidad);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Error al actualizar la entidad: {e.Message}", e);
            }
        }

        public async Task<bool> Delete(int id)
        {
            var entidad = await context.Set<E>()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null) // CORRECCIÓN: Verificar si ES null
            {
                return false;
            }

            try
            {
                context.Set<E>().Remove(entidad);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Error al eliminar la entidad: {e.Message}", e);
            }
        }

        // Métodos adicionales útiles
        public virtual IQueryable<E> GetQueryable()
        {
            return context.Set<E>().AsQueryable();
        }

        public async Task<bool> Existe(Func<E, bool> predicate)
        {
            return await Task.Run(() => context.Set<E>().Any(predicate));
        }

        public async Task<List<E>> Select(Func<E, bool> predicate)
        {
            return await Task.Run(() => context.Set<E>()
                .AsNoTracking()
                .Where(predicate)
                .ToList());
        }
    }
}
