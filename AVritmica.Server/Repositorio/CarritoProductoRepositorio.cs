using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class CarritoProductoRepositorio : ICarritoProductoRepositorio
    {
        private readonly Context _context;

        public CarritoProductoRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<List<CarritoProducto>> Select()
        {
            return await _context.CarritoProductos
                .Include(cp => cp.Carrito)
                .Include(cp => cp.Producto)
                    .ThenInclude(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<CarritoProducto?> SelectById(int id)
        {
            return await _context.CarritoProductos
                .Include(cp => cp.Carrito)
                .Include(cp => cp.Producto)
                    .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CarritoProducto>> SelectByCarrito(int carritoId)
        {
            return await _context.CarritoProductos
                .Include(cp => cp.Carrito)
                .Include(cp => cp.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(x => x.CarritoId == carritoId)
                .ToListAsync();
        }

        public async Task<List<CarritoProducto>> SelectByProducto(int productoId)
        {
            return await _context.CarritoProductos
                .Include(cp => cp.Carrito)
                .Include(cp => cp.Producto)
                    .ThenInclude(p => p.Categoria)
                .Where(x => x.ProductoId == productoId)
                .ToListAsync();
        }

        public async Task<CarritoProducto?> SelectByCarritoAndProducto(int carritoId, int productoId)
        {
            return await _context.CarritoProductos
                .Include(cp => cp.Carrito)
                .Include(cp => cp.Producto)
                    .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(x => x.CarritoId == carritoId && x.ProductoId == productoId);
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.CarritoProductos
                .AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Existe(int carritoId, int productoId)
        {
            return await _context.CarritoProductos
                .AnyAsync(x => x.CarritoId == carritoId && x.ProductoId == productoId);
        }

        public async Task<int> Insert(CarritoProducto entidad)
        {
            // Verificar si ya existe el producto en el carrito
            var existente = await SelectByCarritoAndProducto(entidad.CarritoId, entidad.ProductoId);
            if (existente != null)
            {
                // Si ya existe, actualizar la cantidad
                existente.Cantidad += entidad.Cantidad;
                _context.CarritoProductos.Update(existente);
                await _context.SaveChangesAsync();
                return existente.Id;
            }

            await _context.CarritoProductos.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad.Id;
        }

        public async Task<bool> Update(int id, CarritoProducto entidad)
        {
            var carritoProductoExistente = await _context.CarritoProductos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carritoProductoExistente == null)
                return false;

            // Verificar si se está cambiando el carrito o producto a uno que ya existe
            if (carritoProductoExistente.CarritoId != entidad.CarritoId ||
                carritoProductoExistente.ProductoId != entidad.ProductoId)
            {
                var existe = await Existe(entidad.CarritoId, entidad.ProductoId);
                if (existe && carritoProductoExistente.Id != id)
                {
                    return false; // Ya existe esa combinación carrito-producto
                }
            }

            carritoProductoExistente.CarritoId = entidad.CarritoId;
            carritoProductoExistente.ProductoId = entidad.ProductoId;
            carritoProductoExistente.Cantidad = entidad.Cantidad;
            carritoProductoExistente.PrecioUnitario = entidad.PrecioUnitario;

            _context.CarritoProductos.Update(carritoProductoExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var carritoProducto = await _context.CarritoProductos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carritoProducto == null)
                return false;

            _context.CarritoProductos.Remove(carritoProducto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByCarritoAndProducto(int carritoId, int productoId)
        {
            var carritoProducto = await _context.CarritoProductos
                .FirstOrDefaultAsync(x => x.CarritoId == carritoId && x.ProductoId == productoId);

            if (carritoProducto == null)
                return false;

            _context.CarritoProductos.Remove(carritoProducto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarCantidad(int id, int cantidad)
        {
            var carritoProducto = await _context.CarritoProductos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carritoProducto == null)
                return false;

            carritoProducto.Cantidad = cantidad;

            // No permitir cantidad negativa
            if (carritoProducto.Cantidad < 0)
                carritoProducto.Cantidad = 0;

            _context.CarritoProductos.Update(carritoProducto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarPrecioUnitario(int id, decimal precioUnitario)
        {
            var carritoProducto = await _context.CarritoProductos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carritoProducto == null)
                return false;

            carritoProducto.PrecioUnitario = precioUnitario;

            // No permitir precio negativo
            if (carritoProducto.PrecioUnitario < 0)
                carritoProducto.PrecioUnitario = 0;

            _context.CarritoProductos.Update(carritoProducto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ObtenerCantidadTotalEnCarritos(int productoId)
        {
            return await _context.CarritoProductos
                .Where(x => x.ProductoId == productoId)
                .SumAsync(x => x.Cantidad);
        }
    }
}