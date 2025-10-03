using AVritmica.BD.Data;
using Microsoft.EntityFrameworkCore;

namespace AVritmica.Server.Repositorio
{
    public class Repositorio<E> : IRepositorio<E>
                 where E : class, IEntityBase
    {
        private readonly Context context;

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
            return await context.Set<E>().ToListAsync();
        }

        public async Task<E> SelectById(int id)
        {
            E? m = await context.Set<E>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                x => x.Id == id);
            return m;
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
                throw e;
            }
        }

        public async Task<bool> Update(int id, E entidad)
        {
            if (id != entidad.Id)
            {
                return false;
            }
            var j = await SelectById(id);
            //.Where(reg => reg.Id == id)
            //.FirstOrDefaultAsync();

            if (j == null)
            {
                return false;
            }
            try
            {
                context.Set<E>().Update(entidad);
                await context.SaveChangesAsync();
                return true;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var A = await SelectById(id);

            if (A != null)
            {
                return false;
            }

            context.Set<E>().Remove(A);
            await context.SaveChangesAsync();
            return true;

        }

    }
}
