using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class CompraRepositorio : ICompraRepositorio
    {
        private readonly Context _context;

        public CompraRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<List<Compra>> Select()
        {
            return await _context.Compras
                .Include(c => c.CompraDetalles)
                    .ThenInclude(cd => cd.Producto)
                        .ThenInclude(p => p.Categoria)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<Compra?> SelectById(int id)
        {
            return await _context.Compras
                .Include(c => c.CompraDetalles)
                    .ThenInclude(cd => cd.Producto)
                        .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Compra>> SelectByFecha(DateTime fecha)
        {
            return await _context.Compras
                .Include(c => c.CompraDetalles)
                    .ThenInclude(cd => cd.Producto)
                        .ThenInclude(p => p.Categoria)
                .Where(x => x.FechaCompra.Date == fecha.Date)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<List<Compra>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Compras
                .Include(c => c.CompraDetalles)
                    .ThenInclude(cd => cd.Producto)
                        .ThenInclude(p => p.Categoria)
                .Where(x => x.FechaCompra.Date >= fechaInicio.Date && x.FechaCompra.Date <= fechaFin.Date)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Compras
                .AnyAsync(x => x.Id == id);
        }

        public async Task<int> Insert(Compra entidad)
        {
            await _context.Compras.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad.Id;
        }

        public async Task<bool> Update(int id, Compra entidad)
        {
            var compraExistente = await _context.Compras
                .FirstOrDefaultAsync(x => x.Id == id);

            if (compraExistente == null)
                return false;

            compraExistente.FechaCompra = entidad.FechaCompra;
            compraExistente.Descripcion = entidad.Descripcion;

            _context.Compras.Update(compraExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.CompraDetalles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (compra == null)
                return false;

            // Eliminar los detalles de la compra primero
            _context.CompraDetalles.RemoveRange(compra.CompraDetalles);

            // Eliminar la compra
            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> ObtenerTotalCompra(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.CompraDetalles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (compra == null)
                return 0;

            // Usar PrecioCompra en lugar de PrecioUnitario
            return compra.CompraDetalles.Sum(cd => cd.Cantidad * cd.PrecioCompra);
        }

        public async Task<int> ObtenerCantidadTotalProductos(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.CompraDetalles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (compra == null)
                return 0;

            return compra.CompraDetalles.Sum(cd => cd.Cantidad);
        }
    }
}
