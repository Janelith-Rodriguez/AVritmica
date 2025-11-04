using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;
using AVritmica.Shared.DTO;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class ReportesRepositorio : IReportesRepositorio
    {
        private readonly Context _context;

        public ReportesRepositorio(Context context)
        {
            _context = context;
        }

        // VENTAS
        public async Task<List<ReporteVentasDTO>> ObtenerVentasPorPeriodo(DateTime fechaInicio, DateTime fechaFin)
        {
            var carritos = await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Producto)
                        .ThenInclude(p => p.Categoria)
                .Include(c => c.Pagos)
                .Where(c => c.FechaCreacion >= fechaInicio && c.FechaCreacion <= fechaFin)
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();

            return carritos.Select(c => new ReporteVentasDTO
            {
                Fecha = c.FechaCreacion,
                CarritoId = c.Id,
                Cliente = $"{c.Usuario?.Nombre} {c.Usuario?.Apellido}",
                Estado = c.Estado,
                EstadoPago = c.EstadoPago,
                CantidadProductos = c.CarritoProductos.Count,
                MontoTotal = c.MontoTotal,
                MontoPagado = c.Pagos.Sum(p => p.MontoPagado),
                Saldo = c.Saldo,
                Detalles = c.CarritoProductos.Select(cp => new DetalleVentaDTO
                {
                    Producto = cp.Producto?.Nombre ?? "Producto no disponible",
                    Categoria = cp.Producto?.Categoria?.Nombre ?? "Sin categoría",
                    Cantidad = cp.Cantidad,
                    PrecioUnitario = cp.PrecioUnitario,
                    Subtotal = cp.Cantidad * cp.PrecioUnitario
                }).ToList()
            }).ToList();
        }

        public async Task<ResumenVentasDTO> ObtenerResumenVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            var ventas = await ObtenerVentasPorPeriodo(fechaInicio, fechaFin);

            var resumen = new ResumenVentasDTO
            {
                TotalVentas = ventas.Count,
                VentasConfirmadas = ventas.Count(v => v.Estado == "Confirmado"),
                VentasPendientes = ventas.Count(v => v.Estado == "Activo"),
                TotalMontoVentas = ventas.Sum(v => v.MontoTotal),
                TotalMontoPagado = ventas.Sum(v => v.MontoPagado),
                TotalSaldoPendiente = ventas.Sum(v => v.Saldo),
                TotalProductosVendidos = ventas.Sum(v => v.Detalles.Sum(d => d.Cantidad))
            };

            // Ventas por periodo (mensual)
            var ventasPorMes = ventas
                .GroupBy(v => new { v.Fecha.Year, v.Fecha.Month })
                .Select(g => new VentasPorPeriodoDTO
                {
                    Periodo = $"{g.Key.Month:00}/{g.Key.Year}",
                    CantidadVentas = g.Count(),
                    TotalVentas = g.Sum(v => v.MontoTotal)
                })
                .ToList();

            resumen.VentasPorPeriodo = ventasPorMes;

            return resumen;
        }

        public async Task<List<ReporteVentasDTO>> ObtenerVentasPorUsuario(int usuarioId)
        {
            var carritos = await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Producto)
                        .ThenInclude(p => p.Categoria)
                .Include(c => c.Pagos)
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();

            return carritos.Select(c => new ReporteVentasDTO
            {
                Fecha = c.FechaCreacion,
                CarritoId = c.Id,
                Cliente = $"{c.Usuario?.Nombre} {c.Usuario?.Apellido}",
                Estado = c.Estado,
                EstadoPago = c.EstadoPago,
                CantidadProductos = c.CarritoProductos.Count,
                MontoTotal = c.MontoTotal,
                MontoPagado = c.Pagos.Sum(p => p.MontoPagado),
                Saldo = c.Saldo,
                Detalles = c.CarritoProductos.Select(cp => new DetalleVentaDTO
                {
                    Producto = cp.Producto?.Nombre ?? "Producto no disponible",
                    Categoria = cp.Producto?.Categoria?.Nombre ?? "Sin categoría",
                    Cantidad = cp.Cantidad,
                    PrecioUnitario = cp.PrecioUnitario,
                    Subtotal = cp.Cantidad * cp.PrecioUnitario
                }).ToList()
            }).ToList();
        }

        public async Task<List<ReporteVentasDTO>> ObtenerVentasPorEstado(string estado)
        {
            var carritos = await _context.Carritos
                .Include(c => c.Usuario)
                .Include(c => c.CarritoProductos)
                    .ThenInclude(cp => cp.Producto)
                        .ThenInclude(p => p.Categoria)
                .Include(c => c.Pagos)
                .Where(c => c.Estado == estado)
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();

            return carritos.Select(c => new ReporteVentasDTO
            {
                Fecha = c.FechaCreacion,
                CarritoId = c.Id,
                Cliente = $"{c.Usuario?.Nombre} {c.Usuario?.Apellido}",
                Estado = c.Estado,
                EstadoPago = c.EstadoPago,
                CantidadProductos = c.CarritoProductos.Count,
                MontoTotal = c.MontoTotal,
                MontoPagado = c.Pagos.Sum(p => p.MontoPagado),
                Saldo = c.Saldo,
                Detalles = c.CarritoProductos.Select(cp => new DetalleVentaDTO
                {
                    Producto = cp.Producto?.Nombre ?? "Producto no disponible",
                    Categoria = cp.Producto?.Categoria?.Nombre ?? "Sin categoría",
                    Cantidad = cp.Cantidad,
                    PrecioUnitario = cp.PrecioUnitario,
                    Subtotal = cp.Cantidad * cp.PrecioUnitario
                }).ToList()
            }).ToList();
        }

        // STOCK
        public async Task<List<ReporteStockDTO>> ObtenerReporteStock()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.StockMovimientos)
                .ToListAsync();

            return productos.Select(p => new ReporteStockDTO
            {
                ProductoId = p.Id,
                Producto = p.Nombre,
                Categoria = p.Categoria?.Nombre ?? "Sin categoría",
                StockActual = p.Stock,
                StockMinimo = 10, // Valor por defecto, puedes ajustarlo
                EntradasTotales = p.StockMovimientos
                    .Where(m => m.TipoMovimiento == "Entrada" || m.TipoMovimiento == "Compra" || m.TipoMovimiento == "Ajuste Positivo")
                    .Sum(m => m.Cantidad),
                SalidasTotales = p.StockMovimientos
                    .Where(m => m.TipoMovimiento == "Salida" || m.TipoMovimiento == "Venta" || m.TipoMovimiento == "Ajuste Negativo")
                    .Sum(m => m.Cantidad),
                Precio = p.Precio,
                ValorStock = p.Stock * p.Precio,
                EstadoStock = p.Stock <= 0 ? "Sin Stock" : p.Stock < 10 ? "Stock Bajo" : "Normal",
                UltimosMovimientos = p.StockMovimientos
                    .OrderByDescending(m => m.Fecha)
                    .Take(5)
                    .Select(m => new MovimientoStockDTO
                    {
                        Fecha = m.Fecha,
                        Tipo = m.TipoMovimiento,
                        Cantidad = m.Cantidad,
                        Descripcion = m.Descripcion
                    }).ToList()
            }).ToList();
        }

        public async Task<ResumenStockDTO> ObtenerResumenStock()
        {
            var reporteStock = await ObtenerReporteStock();

            var resumen = new ResumenStockDTO
            {
                TotalProductos = reporteStock.Count,
                ProductosConStockBajo = reporteStock.Count(p => p.StockActual < 10 && p.StockActual > 0),
                ProductosSinStock = reporteStock.Count(p => p.StockActual <= 0),
                TotalStock = reporteStock.Sum(p => p.StockActual),
                ValorTotalStock = reporteStock.Sum(p => p.ValorStock)
            };

            // Movimientos de hoy
            var hoy = DateTime.Today;
            var movimientosHoy = await _context.StockMovimientos
                .CountAsync(m => m.Fecha.Date == hoy);
            resumen.MovimientosHoy = movimientosHoy;

            // Productos con stock bajo
            resumen.ProductosStockBajo = reporteStock
                .Where(p => p.StockActual < 10)
                .Select(p => new ProductosStockBajoDTO
                {
                    Producto = p.Producto,
                    StockActual = p.StockActual,
                    StockMinimo = p.StockMinimo,
                    Categoria = p.Categoria
                })
                .ToList();

            return resumen;
        }

        public async Task<List<ReporteStockDTO>> ObtenerProductosStockBajo()
        {
            var reporteStock = await ObtenerReporteStock();
            return reporteStock.Where(p => p.StockActual < 10).ToList();
        }

        public async Task<List<ReporteStockDTO>> ObtenerMovimientosStockPorProducto(int productoId, DateTime fechaInicio, DateTime fechaFin)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.StockMovimientos)
                .FirstOrDefaultAsync(p => p.Id == productoId);

            if (producto == null)
                return new List<ReporteStockDTO>();

            var movimientosFiltrados = producto.StockMovimientos
                .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
                .OrderByDescending(m => m.Fecha)
                .ToList();

            var reporte = new ReporteStockDTO
            {
                ProductoId = producto.Id,
                Producto = producto.Nombre,
                Categoria = producto.Categoria?.Nombre ?? "Sin categoría",
                StockActual = producto.Stock,
                StockMinimo = 10,
                EntradasTotales = movimientosFiltrados
                    .Where(m => m.TipoMovimiento == "Entrada" || m.TipoMovimiento == "Compra" || m.TipoMovimiento == "Ajuste Positivo")
                    .Sum(m => m.Cantidad),
                SalidasTotales = movimientosFiltrados
                    .Where(m => m.TipoMovimiento == "Salida" || m.TipoMovimiento == "Venta" || m.TipoMovimiento == "Ajuste Negativo")
                    .Sum(m => m.Cantidad),
                Precio = producto.Precio,
                ValorStock = producto.Stock * producto.Precio,
                EstadoStock = producto.Stock <= 0 ? "Sin Stock" : producto.Stock < 10 ? "Stock Bajo" : "Normal",
                UltimosMovimientos = movimientosFiltrados
                    .Select(m => new MovimientoStockDTO
                    {
                        Fecha = m.Fecha,
                        Tipo = m.TipoMovimiento,
                        Cantidad = m.Cantidad,
                        Descripcion = m.Descripcion
                    }).ToList()
            };

            return new List<ReporteStockDTO> { reporte };
        }

        // COMPRAS
        public async Task<List<ReporteComprasDTO>> ObtenerComprasPorPeriodo(DateTime fechaInicio, DateTime fechaFin)
        {
            var compras = await _context.Compras
                .Include(c => c.CompraDetalles)
                    .ThenInclude(cd => cd.Producto)
                        .ThenInclude(p => p.Categoria)
                .Where(c => c.FechaCompra >= fechaInicio && c.FechaCompra <= fechaFin)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();

            return compras.Select(c => new ReporteComprasDTO
            {
                CompraId = c.Id,
                FechaCompra = c.FechaCompra,
                Descripcion = c.Descripcion,
                CantidadProductos = c.CompraDetalles.Count,
                TotalUnidades = c.CompraDetalles.Sum(cd => cd.Cantidad),
                TotalCompra = c.CompraDetalles.Sum(cd => cd.Cantidad * cd.PrecioCompra),
                Detalles = c.CompraDetalles.Select(cd => new DetalleCompraDTO
                {
                    Producto = cd.Producto?.Nombre ?? "Producto no disponible",
                    Categoria = cd.Producto?.Categoria?.Nombre ?? "Sin categoría",
                    Cantidad = cd.Cantidad,
                    PrecioCompra = cd.PrecioCompra,
                    Subtotal = cd.Cantidad * cd.PrecioCompra
                }).ToList()
            }).ToList();
        }

        public async Task<ResumenComprasDTO> ObtenerResumenCompras(DateTime fechaInicio, DateTime fechaFin)
        {
            var compras = await ObtenerComprasPorPeriodo(fechaInicio, fechaFin);

            var resumen = new ResumenComprasDTO
            {
                TotalCompras = compras.Count,
                TotalProductosComprados = compras.Sum(c => c.CantidadProductos),
                TotalUnidadesCompradas = compras.Sum(c => c.TotalUnidades),
                TotalInvertido = compras.Sum(c => c.TotalCompra),
                PromedioCompra = compras.Any() ? compras.Average(c => c.TotalCompra) : 0
            };

            // Compras por periodo (mensual)
            var comprasPorMes = compras
                .GroupBy(c => new { c.FechaCompra.Year, c.FechaCompra.Month })
                .Select(g => new ComprasPorPeriodoDTO
                {
                    Periodo = $"{g.Key.Month:00}/{g.Key.Year}",
                    CantidadCompras = g.Count(),
                    TotalCompras = g.Sum(c => c.TotalCompra)
                })
                .ToList();

            resumen.ComprasPorPeriodo = comprasPorMes;

            return resumen;
        }

        // USUARIOS
        public async Task<List<ReporteUsuariosDTO>> ObtenerReporteUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Carritos)
                .Include(u => u.Consultas)
                .ToListAsync();

            return usuarios.Select(u => new ReporteUsuariosDTO
            {
                UsuarioId = u.Id,
                NombreCompleto = $"{u.Nombre} {u.Apellido}",
                Email = u.Email,
                TipoUsuario = u.TipoUsuario,
                FechaRegistro = DateTime.Now, // Ajusta según tu EntityBase
                TotalCarritos = u.Carritos.Count,
                CarritosActivos = u.Carritos.Count(c => c.Estado == "Activo"),
                CarritosConfirmados = u.Carritos.Count(c => c.Estado == "Confirmado"),
                TotalCompras = u.Carritos.Sum(c => c.MontoTotal),
                ConsultasRealizadas = u.Consultas.Count,
                Telefono = u.Telefono
            }).ToList();
        }

        public async Task<ResumenUsuariosDTO> ObtenerResumenUsuarios()
        {
            var reporteUsuarios = await ObtenerReporteUsuarios();

            var resumen = new ResumenUsuariosDTO
            {
                TotalUsuarios = reporteUsuarios.Count,
                UsuariosClientes = reporteUsuarios.Count(u => u.TipoUsuario == "Cliente"),
                UsuariosAdministradores = reporteUsuarios.Count(u => u.TipoUsuario == "Administrador"),
                UsuariosVendedores = reporteUsuarios.Count(u => u.TipoUsuario == "Vendedor"),
                TotalCarritos = reporteUsuarios.Sum(u => u.TotalCarritos),
                CarritosActivos = reporteUsuarios.Sum(u => u.CarritosActivos),
                TotalVentasUsuarios = reporteUsuarios.Sum(u => u.TotalCompras)
            };

            // Usuarios más activos (top 5 por total de compras)
            resumen.UsuariosMasActivos = reporteUsuarios
                .OrderByDescending(u => u.TotalCompras)
                .Take(5)
                .Select(u => new UsuariosActivosDTO
                {
                    Usuario = u.NombreCompleto,
                    CarritosTotales = u.TotalCarritos,
                    TotalCompras = u.TotalCompras
                })
                .ToList();

            return resumen;
        }

        public async Task<List<ReporteUsuariosDTO>> ObtenerUsuariosPorTipo(string tipoUsuario)
        {
            var reporteUsuarios = await ObtenerReporteUsuarios();
            return reporteUsuarios.Where(u => u.TipoUsuario == tipoUsuario).ToList();
        }
    }
}
