using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class PagoRepositorio : IPagoRepositorio
    {
        private readonly Context _context;

        public PagoRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<List<Pago>> Select()
        {
            return await _context.Pagos
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                        .ThenInclude(cp => cp.Producto)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<Pago?> SelectById(int id)
        {
            return await _context.Pagos
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                        .ThenInclude(cp => cp.Producto)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Pago>> SelectByCarrito(int carritoId)
        {
            return await _context.Pagos
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                        .ThenInclude(cp => cp.Producto)
                .Where(x => x.CarritoId == carritoId)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<List<Pago>> SelectByEstado(string estadoPago)
        {
            return await _context.Pagos
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                        .ThenInclude(cp => cp.Producto)
                .Where(x => x.EstadoPago == estadoPago)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<List<Pago>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Pagos
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                        .ThenInclude(cp => cp.Producto)
                .Where(x => x.FechaPago >= fechaInicio && x.FechaPago <= fechaFin)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<List<Pago>> SelectByMetodoPago(string metodoPago)
        {
            return await _context.Pagos
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.Usuario)
                .Include(p => p.Carrito)
                    .ThenInclude(c => c.CarritoProductos)
                        .ThenInclude(cp => cp.Producto)
                .Where(x => x.MetodoPago == metodoPago)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<decimal> ObtenerTotalPagadoPorCarrito(int carritoId)
        {
            return await _context.Pagos
                .Where(x => x.CarritoId == carritoId && x.EstadoPago == "Completado")
                .SumAsync(x => x.MontoPagado);
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Pagos
                .AnyAsync(x => x.Id == id);
        }

        public async Task<int> Insert(Pago entidad)
        {
            // Asegurar que la fecha de pago sea la actual
            entidad.FechaPago = DateTime.UtcNow;

            await _context.Pagos.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad.Id;
        }

        public async Task<bool> Update(int id, Pago entidad)
        {
            var pagoExistente = await _context.Pagos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pagoExistente == null)
                return false;

            pagoExistente.CarritoId = entidad.CarritoId;
            pagoExistente.MetodoPago = entidad.MetodoPago;
            pagoExistente.MontoPagado = entidad.MontoPagado;
            pagoExistente.EstadoPago = entidad.EstadoPago;
            pagoExistente.Saldo = entidad.Saldo;
            // No actualizamos FechaPago para mantener la fecha original

            _context.Pagos.Update(pagoExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var pago = await _context.Pagos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pago == null)
                return false;

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarEstadoPago(int id, string estadoPago)
        {
            var pago = await _context.Pagos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pago == null)
                return false;

            pago.EstadoPago = estadoPago;
            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProcesarPago(int carritoId, string metodoPago, decimal montoPagado)
        {
            // Obtener el carrito
            var carrito = await _context.Carritos
                .Include(c => c.Pagos)
                .FirstOrDefaultAsync(x => x.Id == carritoId);

            if (carrito == null)
                return false;

            // Calcular el saldo pendiente
            var totalPagado = carrito.Pagos
                .Where(p => p.EstadoPago == "Completado")
                .Sum(p => p.MontoPagado);
            var saldoPendiente = carrito.MontoTotal - totalPagado;

            // Verificar que el monto pagado no exceda el saldo pendiente
            if (montoPagado > saldoPendiente)
            {
                return false;
            }

            // Crear el nuevo pago
            var pago = new Pago
            {
                CarritoId = carritoId,
                MetodoPago = metodoPago,
                MontoPagado = montoPagado,
                EstadoPago = "Completado",
                Saldo = saldoPendiente - montoPagado,
                FechaPago = DateTime.UtcNow
            };

            // Actualizar el saldo del carrito
            carrito.Saldo = pago.Saldo;

            // Si el saldo llega a cero, marcar el pago como completado
            if (carrito.Saldo <= 0)
            {
                carrito.EstadoPago = "Completado";
            }

            await _context.Pagos.AddAsync(pago);
            _context.Carritos.Update(carrito);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> ObtenerSaldoPendientePorCarrito(int carritoId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Pagos)
                .FirstOrDefaultAsync(x => x.Id == carritoId);

            if (carrito == null)
                return 0;

            var totalPagado = carrito.Pagos
                .Where(p => p.EstadoPago == "Completado")
                .Sum(p => p.MontoPagado);

            return carrito.MontoTotal - totalPagado;
        }
    }
}
