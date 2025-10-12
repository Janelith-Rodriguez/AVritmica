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
            // Asumiendo que tendrías un campo "Leida" en tu entidad Consulta
            // Si no lo tienes, podemos agregarlo o usar otro criterio
            return await _context.Consultas
                .Include(c => c.Usuario)
                .Where(x => !x.Leida) // Asumiendo que existe la propiedad Leida
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
            // Asegurar que la fecha de envío sea la actual
            entidad.FechaEnvio = DateTime.UtcNow;

            await _context.Consultas.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad.Id;
        }

        public async Task<bool> Update(int id, Consulta entidad)
        {
            var consultaExistente = await _context.Consultas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (consultaExistente == null)
                return false;

            consultaExistente.UsuarioId = entidad.UsuarioId;
            consultaExistente.Nombre = entidad.Nombre;
            consultaExistente.Email = entidad.Email;
            consultaExistente.Mensaje = entidad.Mensaje;
            // No actualizamos FechaEnvio para mantener la fecha original

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

            consulta.Leida = true; // Asumiendo que existe la propiedad Leida
            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ObtenerCantidadNoLeidas()
        {
            return await _context.Consultas
                .Where(x => !x.Leida) // Asumiendo que existe la propiedad Leida
                .CountAsync();
        }
    }
}
