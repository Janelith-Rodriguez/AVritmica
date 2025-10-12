using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class CompraDetalleRepositorio : ICompraDetalleRepositorio
    {
        private readonly Context _context;

        public CompraDetalleRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<List<CompraDetalle>> Select()
        {
            return await _context.CompraDetalles
                .Include(cd => cd.Compra)
                .Include(cd => cd.Producto)
                    .ThenInclude(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<CompraDetalle?> SelectById(int id)
        {
            return await _context.CompraDetalles
                .Include(cd => cd.Compra)
                .Include(cd => cd.Producto)
                    .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CompraDetalle>> SelectByCompra(int compraId)
        {
            return await _context.CompraDetalles
                .Include(cd => cd.Compra)
                .Include(cd => cd.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(x => x.CompraId == compraId)
                .ToListAsync();
        }

        public async Task<List<CompraDetalle>> SelectByProducto(int productoId)
        {
            return await _context.CompraDetalles
                .Include(cd => cd.Compra)
                .Include(cd => cd.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(x => x.ProductoId == productoId)
                .OrderByDescending(x => x.Compra.FechaCompra)
                .ToListAsync();
        }

        public async Task<CompraDetalle?> SelectByCompraAndProducto(int compraId, int productoId)
        {
            return await _context.CompraDetalles
                .Include(cd => cd.Compra)
                .Include(cd => cd.Producto)
                    .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(x => x.CompraId == compraId && x.ProductoId == productoId);
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.CompraDetalles
                .AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Existe(int compraId, int productoId)
        {
            return await _context.CompraDetalles
                .AnyAsync(x => x.CompraId == compraId && x.ProductoId == productoId);
        }

        public async Task<int> Insert(CompraDetalle entidad)
        {
            // Verificar si ya existe el producto en la compra
            var existente = await SelectByCompraAndProducto(entidad.CompraId, entidad.ProductoId);
            if (existente != null)
            {
                // Si ya existe, actualizar la cantidad y precios
                existente.Cantidad += entidad.Cantidad;
                existente.PrecioCompra = entidad.PrecioCompra;
                existente.PrecioVentaActualizado = entidad.PrecioVentaActualizado;

                _context.CompraDetalles.Update(existente);
                await _context.SaveChangesAsync();
                return existente.Id;
            }

            await _context.CompraDetalles.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad.Id;
        }

        public async Task<bool> Update(int id, CompraDetalle entidad)
        {
            var compraDetalleExistente = await _context.CompraDetalles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (compraDetalleExistente == null)
                return false;

            // Verificar si se está cambiando la compra o producto a uno que ya existe
            if (compraDetalleExistente.CompraId != entidad.CompraId ||
                compraDetalleExistente.ProductoId != entidad.ProductoId)
            {
                var existe = await Existe(entidad.CompraId, entidad.ProductoId);
                if (existe)
                {
                    return false; // Ya existe esa combinación compra-producto
                }
            }

            compraDetalleExistente.CompraId = entidad.CompraId;
            compraDetalleExistente.ProductoId = entidad.ProductoId;
            compraDetalleExistente.Cantidad = entidad.Cantidad;
            compraDetalleExistente.PrecioCompra = entidad.PrecioCompra;
            compraDetalleExistente.PrecioVentaActualizado = entidad.PrecioVentaActualizado;

            _context.CompraDetalles.Update(compraDetalleExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var compraDetalle = await _context.CompraDetalles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (compraDetalle == null)
                return false;

            _context.CompraDetalles.Remove(compraDetalle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByCompraAndProducto(int compraId, int productoId)
        {
            var compraDetalle = await _context.CompraDetalles
                .FirstOrDefaultAsync(x => x.CompraId == compraId && x.ProductoId == productoId);

            if (compraDetalle == null)
                return false;

            _context.CompraDetalles.Remove(compraDetalle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarCantidad(int id, int cantidad)
        {
            var compraDetalle = await _context.CompraDetalles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (compraDetalle == null)
                return false;

            compraDetalle.Cantidad = cantidad;

            // No permitir cantidad negativa
            if (compraDetalle.Cantidad < 0)
                compraDetalle.Cantidad = 0;

            _context.CompraDetalles.Update(compraDetalle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarPrecios(int id, decimal precioCompra, decimal precioVentaActualizado)
        {
            var compraDetalle = await _context.CompraDetalles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (compraDetalle == null)
                return false;

            compraDetalle.PrecioCompra = precioCompra;
            compraDetalle.PrecioVentaActualizado = precioVentaActualizado;

            // No permitir precios negativos
            if (compraDetalle.PrecioCompra < 0)
                compraDetalle.PrecioCompra = 0;
            if (compraDetalle.PrecioVentaActualizado < 0)
                compraDetalle.PrecioVentaActualizado = 0;

            _context.CompraDetalles.Update(compraDetalle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> ObtenerTotalPorCompra(int compraId)
        {
            return await _context.CompraDetalles
                .Where(x => x.CompraId == compraId)
                .SumAsync(x => x.Cantidad * x.PrecioCompra);
        }

        public async Task<int> ObtenerCantidadTotalPorProducto(int productoId)
        {
            return await _context.CompraDetalles
                .Where(x => x.ProductoId == productoId)
                .SumAsync(x => x.Cantidad);
        }

        public async Task<List<CompraDetalle>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.CompraDetalles
                .Include(cd => cd.Compra)
                .Include(cd => cd.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(x => x.Compra.FechaCompra >= fechaInicio && x.Compra.FechaCompra <= fechaFin)
                .OrderByDescending(x => x.Compra.FechaCompra)
                .ToListAsync();
        }
    }
}
