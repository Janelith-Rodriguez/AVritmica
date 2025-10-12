using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class StockMovimientoRepositorio : IStockMovimientoRepositorio
    {
        private readonly Context _context;

        public StockMovimientoRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<List<StockMovimiento>> Select()
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<StockMovimiento?> SelectById(int id)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<StockMovimiento>> SelectByProducto(int productoId)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(x => x.ProductoId == productoId)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<List<StockMovimiento>> SelectByTipoMovimiento(string tipoMovimiento)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(x => x.TipoMovimiento == tipoMovimiento)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<List<StockMovimiento>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<List<StockMovimiento>> SelectByProductoAndRangoFechas(int productoId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(x => x.ProductoId == productoId && x.Fecha >= fechaInicio && x.Fecha <= fechaFin)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.StockMovimientos
                .AnyAsync(x => x.Id == id);
        }

        public async Task<int> Insert(StockMovimiento entidad)
        {
            // Asegurar que la fecha sea la actual si no se especifica
            if (entidad.Fecha == default)
            {
                entidad.Fecha = DateTime.UtcNow;
            }

            await _context.StockMovimientos.AddAsync(entidad);
            await _context.SaveChangesAsync();

            // Actualizar el stock del producto
            await ActualizarStockProducto(entidad.ProductoId);

            return entidad.Id;
        }

        public async Task<bool> Update(int id, StockMovimiento entidad)
        {
            var movimientoExistente = await _context.StockMovimientos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movimientoExistente == null)
                return false;

            // Guardar los valores antiguos para recalcular el stock
            var productoIdAnterior = movimientoExistente.ProductoId;
            var cantidadAnterior = movimientoExistente.Cantidad;
            var tipoMovimientoAnterior = movimientoExistente.TipoMovimiento;

            movimientoExistente.ProductoId = entidad.ProductoId;
            movimientoExistente.TipoMovimiento = entidad.TipoMovimiento;
            movimientoExistente.Cantidad = entidad.Cantidad;
            movimientoExistente.Descripcion = entidad.Descripcion;
            // No actualizamos la fecha para mantener la fecha original del movimiento

            _context.StockMovimientos.Update(movimientoExistente);
            await _context.SaveChangesAsync();

            // Recalcular el stock para ambos productos (anterior y actual) si cambiaron
            if (productoIdAnterior != entidad.ProductoId)
            {
                await ActualizarStockProducto(productoIdAnterior);
            }
            await ActualizarStockProducto(entidad.ProductoId);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var movimiento = await _context.StockMovimientos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movimiento == null)
                return false;

            var productoId = movimiento.ProductoId;

            _context.StockMovimientos.Remove(movimiento);
            await _context.SaveChangesAsync();

            // Actualizar el stock del producto después de eliminar el movimiento
            await ActualizarStockProducto(productoId);

            return true;
        }

        public async Task<int> ObtenerStockActual(int productoId)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(x => x.Id == productoId);

            return producto?.Stock ?? 0;
        }

        public async Task<int> ObtenerEntradasTotales(int productoId)
        {
            return await _context.StockMovimientos
                .Where(x => x.ProductoId == productoId &&
                           (x.TipoMovimiento == "Entrada" || x.TipoMovimiento == "Compra" || x.TipoMovimiento == "Ajuste Positivo"))
                .SumAsync(x => x.Cantidad);
        }

        public async Task<int> ObtenerSalidasTotales(int productoId)
        {
            return await _context.StockMovimientos
                .Where(x => x.ProductoId == productoId &&
                           (x.TipoMovimiento == "Salida" || x.TipoMovimiento == "Venta" || x.TipoMovimiento == "Ajuste Negativo"))
                .SumAsync(x => x.Cantidad);
        }

        public async Task<bool> RegistrarMovimiento(int productoId, string tipoMovimiento, int cantidad, string descripcion = "")
        {
            try
            {
                var movimiento = new StockMovimiento
                {
                    ProductoId = productoId,
                    TipoMovimiento = tipoMovimiento,
                    Cantidad = cantidad,
                    Descripcion = descripcion,
                    Fecha = DateTime.UtcNow
                };

                await _context.StockMovimientos.AddAsync(movimiento);
                await _context.SaveChangesAsync();

                // Actualizar el stock del producto
                await ActualizarStockProducto(productoId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task ActualizarStockProducto(int productoId)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(x => x.Id == productoId);

            if (producto != null)
            {
                var entradas = await ObtenerEntradasTotales(productoId);
                var salidas = await ObtenerSalidasTotales(productoId);

                producto.Stock = entradas - salidas;

                // No permitir stock negativo
                if (producto.Stock < 0)
                    producto.Stock = 0;

                _context.Productos.Update(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
