using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class ConsultaRepositorio : IConsultaRepositorio
    {
        private readonly Context _context;

        public ConsultaRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<List<Consulta>> Select()
        {
            return await _context.Consultas
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.FechaEnvio)
                .ToListAsync();
        }

        public async Task<Consulta?> SelectById(int id)
        {
            return await _context.Consultas
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Consulta>> SelectByUsuario(int usuarioId)
        {
            return await _context.Consultas
                .Include(c => c.Usuario)
                .Where(x => x.UsuarioId == usuarioId)
                .OrderByDescending(c => c.FechaEnvio)
                .ToListAsync();
        }

        public async Task<List<Consulta>> SelectByEmail(string email)
        {
            return await _context.Consultas
                .Include(c => c.Usuario)
                .Where(x => x.Email == email)
                .OrderByDescending(c => c.FechaEnvio)
                .ToListAsync();
        }

        public async Task<List<Consulta>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Consultas
                .Include(c => c.Usuario)
                .Where(x => x.FechaEnvio >= fechaInicio && x.FechaEnvio <= fechaFin)
                .OrderByDescending(c => c.FechaEnvio)
                .ToListAsync();
        }

        public async Task<List<Consulta>> SelectNoLeidas()
        {
            return await _context.Consultas
                .Include(c => c.Usuario)
                .Where(x => !(x.Leida ?? false))
                .OrderByDescending(c => c.FechaEnvio)
                .ToListAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Consultas
                .AnyAsync(x => x.Id == id);
        }

        public async Task<int> Insert(Consulta entidad)
        {
            try
            {
                // Asegurar valores por defecto
                entidad.FechaEnvio = DateTime.UtcNow;

                // Si UsuarioId es 0, establecer como null
                if (entidad.UsuarioId == 0)
                {
                    entidad.UsuarioId = null;
                }

                // Verificar si el usuario existe si se proporciona un ID
                if (entidad.UsuarioId.HasValue && entidad.UsuarioId.Value > 0)
                {
                    var usuarioExiste = await _context.Usuarios
                        .AnyAsync(u => u.Id == entidad.UsuarioId.Value);

                    if (!usuarioExiste)
                    {
                        entidad.UsuarioId = null;
                    }
                }

                if (!entidad.Leida.HasValue)
                {
                    entidad.Leida = false;
                }

                await _context.Consultas.AddAsync(entidad);
                await _context.SaveChangesAsync();
                return entidad.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Insert: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw;
            }
        }

        // NUEVO MÉTODO: Específico para consultas públicas
        public async Task<int> InsertConsultaPublica(string nombre, string email, string mensaje)
        {
            try
            {
                var consulta = new Consulta
                {
                    Nombre = nombre,
                    Email = email,
                    Mensaje = mensaje,
                    FechaEnvio = DateTime.UtcNow,
                    UsuarioId = null, // Siempre null para consultas públicas
                    Leida = false
                };

                await _context.Consultas.AddAsync(consulta);
                await _context.SaveChangesAsync();
                return consulta.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en InsertConsultaPublica: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> Update(int id, Consulta entidad)
        {
            var consultaExistente = await _context.Consultas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (consultaExistente == null)
                return false;

            consultaExistente.Nombre = entidad.Nombre;
            consultaExistente.Email = entidad.Email;
            consultaExistente.Mensaje = entidad.Mensaje;
            consultaExistente.Leida = entidad.Leida;

            _context.Consultas.Update(consultaExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (consulta == null)
                return false;

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarcarComoLeida(int id)
        {
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (consulta == null)
                return false;

            consulta.Leida = true;
            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ObtenerCantidadNoLeidas()
        {
            return await _context.Consultas
                .Where(x => !(x.Leida ?? false))
                .CountAsync();
        }
    }
}
