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
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                .Include(sm => sm.Compra)  // NUEVO: Incluir compra
                    .ThenInclude(c => c.CompraDetalles)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<StockMovimiento?> SelectById(int id)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                .Include(sm => sm.Compra)  // NUEVO: Incluir compra
                    .ThenInclude(c => c.CompraDetalles)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<StockMovimiento>> SelectByProducto(int productoId)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(sm => sm.Compra)  // NUEVO
                    .ThenInclude(c => c.CompraDetalles)
                .Where(x => x.ProductoId == productoId)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<List<StockMovimiento>> SelectByTipoMovimiento(string tipoMovimiento)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(sm => sm.Compra)  // NUEVO
                    .ThenInclude(c => c.CompraDetalles)
                .Where(x => x.TipoMovimiento == tipoMovimiento)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<List<StockMovimiento>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(sm => sm.Compra)  // NUEVO
                    .ThenInclude(c => c.CompraDetalles)
                .Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<List<StockMovimiento>> SelectByProductoAndRangoFechas(int productoId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(sm => sm.Compra)  // NUEVO
                    .ThenInclude(c => c.CompraDetalles)
                .Where(x => x.ProductoId == productoId && x.Fecha >= fechaInicio && x.Fecha <= fechaFin)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<List<StockMovimiento>> SelectByCarrito(int carritoId)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(sm => sm.Compra)  // NUEVO
                    .ThenInclude(c => c.CompraDetalles)
                .Where(x => x.CarritoId == carritoId)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        public async Task<List<StockMovimiento>> SelectByProveedor(string proveedor)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(sm => sm.Compra)  // NUEVO
                    .ThenInclude(c => c.CompraDetalles)
                .Where(x => x.Proveedor == proveedor)
                .OrderByDescending(sm => sm.Fecha)
                .ToListAsync();
        }

        // NUEVO MÉTODO: Obtener movimientos por compra
        public async Task<List<StockMovimiento>> SelectByCompra(int compraId)
        {
            return await _context.StockMovimientos
                .Include(sm => sm.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(sm => sm.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(sm => sm.Compra)
                    .ThenInclude(c => c.CompraDetalles)
                .Where(x => x.CompraId == compraId)
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
            if (entidad.Fecha == default)
            {
                entidad.Fecha = DateTime.UtcNow;
            }

            await _context.StockMovimientos.AddAsync(entidad);
            await _context.SaveChangesAsync();

            await ActualizarStockProducto(entidad.ProductoId);
            return entidad.Id;
        }

        public async Task<bool> Update(int id, StockMovimiento entidad)
        {
            var movimientoExistente = await _context.StockMovimientos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movimientoExistente == null)
                return false;

            var productoIdAnterior = movimientoExistente.ProductoId;
            var cantidadAnterior = movimientoExistente.Cantidad;
            var tipoMovimientoAnterior = movimientoExistente.TipoMovimiento;

            // Actualizar propiedades
            movimientoExistente.ProductoId = entidad.ProductoId;
            movimientoExistente.TipoMovimiento = entidad.TipoMovimiento;
            movimientoExistente.Cantidad = entidad.Cantidad;
            movimientoExistente.Descripcion = entidad.Descripcion;
            movimientoExistente.CarritoId = entidad.CarritoId;
            movimientoExistente.CompraId = entidad.CompraId;  // NUEVO
            movimientoExistente.Proveedor = entidad.Proveedor;
            movimientoExistente.NumeroFactura = entidad.NumeroFactura;
            movimientoExistente.NumeroOrdenCompra = entidad.NumeroOrdenCompra;
            movimientoExistente.UsuarioRegistro = entidad.UsuarioRegistro;
            movimientoExistente.FechaDocumento = entidad.FechaDocumento;

            _context.StockMovimientos.Update(movimientoExistente);
            await _context.SaveChangesAsync();

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

                await ActualizarStockProducto(productoId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RegistrarMovimientoConCarrito(int productoId, int carritoId, string tipoMovimiento, int cantidad, string descripcion = "")
        {
            try
            {
                var movimiento = new StockMovimiento
                {
                    ProductoId = productoId,
                    CarritoId = carritoId,
                    TipoMovimiento = tipoMovimiento,
                    Cantidad = cantidad,
                    Descripcion = descripcion,
                    Fecha = DateTime.UtcNow
                };

                await _context.StockMovimientos.AddAsync(movimiento);
                await _context.SaveChangesAsync();

                await ActualizarStockProducto(productoId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RegistrarMovimientoConProveedor(int productoId, string proveedor, string tipoMovimiento, int cantidad, string numeroFactura = "", string descripcion = "")
        {
            try
            {
                var movimiento = new StockMovimiento
                {
                    ProductoId = productoId,
                    Proveedor = proveedor,
                    NumeroFactura = numeroFactura,
                    TipoMovimiento = tipoMovimiento,
                    Cantidad = cantidad,
                    Descripcion = descripcion,
                    Fecha = DateTime.UtcNow
                };

                await _context.StockMovimientos.AddAsync(movimiento);
                await _context.SaveChangesAsync();

                await ActualizarStockProducto(productoId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // NUEVO MÉTODO: Registrar movimiento con compra
        public async Task<bool> RegistrarMovimientoConCompra(int productoId, int compraId, string tipoMovimiento, int cantidad, string descripcion = "")
        {
            try
            {
                var movimiento = new StockMovimiento
                {
                    ProductoId = productoId,
                    CompraId = compraId,
                    TipoMovimiento = tipoMovimiento,
                    Cantidad = cantidad,
                    Descripcion = descripcion,
                    Fecha = DateTime.UtcNow
                };

                await _context.StockMovimientos.AddAsync(movimiento);
                await _context.SaveChangesAsync();

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

                if (producto.Stock < 0)
                    producto.Stock = 0;

                _context.Productos.Update(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
