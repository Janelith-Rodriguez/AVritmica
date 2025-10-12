using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class CarritoRepositorio : ICarritoRepositorio
    {
        private readonly Context _context;

        public CarritoRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<List<Carrito>> Select()
        {
            return await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Producto)
                .Include(c => c.Pagos)
                .ToListAsync();
        }

        public async Task<Carrito?> SelectById(int id)
        {
            return await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Producto)
                .Include(c => c.Pagos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Carrito>> SelectByUsuario(int usuarioId)
        {
            return await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Producto)
                .Include(c => c.Pagos)
                .Where(x => x.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<List<Carrito>> SelectByEstado(string estado)
        {
            return await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Producto)
                .Include(c => c.Pagos)
                .Where(x => x.Estado == estado)
                .ToListAsync();
        }

        public async Task<List<Carrito>> SelectByEstadoPago(string estadoPago)
        {
            return await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Producto)
                .Include(c => c.Pagos)
                .Where(x => x.EstadoPago == estadoPago)
                .ToListAsync();
        }

        public async Task<Carrito?> SelectCarritoActivoByUsuario(int usuarioId)
        {
            return await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Producto)
                .Include(c => c.Pagos)
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.Estado == "Activo");
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Carritos
                .AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ExisteCarritoActivo(int usuarioId)
        {
            return await _context.Carritos
                .AnyAsync(x => x.UsuarioId == usuarioId && x.Estado == "Activo");
        }

        public async Task<int> Insert(Carrito entidad)
        {
            await _context.Carritos.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad.Id;
        }

        public async Task<bool> Update(int id, Carrito entidad)
        {
            var carritoExistente = await _context.Carritos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carritoExistente == null)
                return false;

            carritoExistente.UsuarioId = entidad.UsuarioId;
            carritoExistente.Estado = entidad.Estado;
            carritoExistente.EstadoPago = entidad.EstadoPago;
            carritoExistente.MontoTotal = entidad.MontoTotal;
            carritoExistente.Saldo = entidad.Saldo;
            carritoExistente.DireccionEnvio = entidad.DireccionEnvio;
            carritoExistente.FechaConfirmacion = entidad.FechaConfirmacion;

            _context.Carritos.Update(carritoExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var carrito = await _context.Carritos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carrito == null)
                return false;

            _context.Carritos.Remove(carrito);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarEstado(int id, string estado)
        {
            var carrito = await _context.Carritos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carrito == null)
                return false;

            carrito.Estado = estado;

            if (estado == "Confirmado")
            {
                carrito.FechaConfirmacion = DateTime.UtcNow;
            }

            _context.Carritos.Update(carrito);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarEstadoPago(int id, string estadoPago)
        {
            var carrito = await _context.Carritos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carrito == null)
                return false;

            carrito.EstadoPago = estadoPago;
            _context.Carritos.Update(carrito);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmarCarrito(int id, decimal montoTotal, string direccionEnvio)
        {
            var carrito = await _context.Carritos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carrito == null)
                return false;

            carrito.Estado = "Confirmado";
            carrito.MontoTotal = montoTotal;
            carrito.Saldo = montoTotal; // El saldo inicial es igual al monto total
            carrito.DireccionEnvio = direccionEnvio;
            carrito.FechaConfirmacion = DateTime.UtcNow;

            _context.Carritos.Update(carrito);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarMontoTotal(int id, decimal montoTotal)
        {
            var carrito = await _context.Carritos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carrito == null)
                return false;

            carrito.MontoTotal = montoTotal;
            // Actualizar el saldo también si es necesario
            carrito.Saldo = montoTotal - carrito.Pagos.Where(p => p.EstadoPago == "Aprobado").Sum(p => p.MontoPagado);

            _context.Carritos.Update(carrito);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
