using Microsoft.EntityFrameworkCore;
using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;
using AVritmica.Server.Repositorio;

namespace AVritmica.Server.RepositorioImplementacion
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly Context _context;

        public UsuarioRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> Select()
        {
            return await _context.Usuarios
                .Include(u => u.Carritos)
                .Include(u => u.Consultas)
                .ToListAsync();
        }

        public async Task<Usuario?> SelectById(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Carritos)
                .Include(u => u.Consultas)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Usuario?> SelectByEmail(string email)
        {
            return await _context.Usuarios
                .Include(u => u.Carritos)
                .Include(u => u.Consultas)
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<List<Usuario>> SelectByTipoUsuario(string tipoUsuario)
        {
            return await _context.Usuarios
                .Include(u => u.Carritos)
                .Include(u => u.Consultas)
                .Where(x => x.TipoUsuario == tipoUsuario)
                .ToListAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Usuarios
                .AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ExisteEmail(string email)
        {
            return await _context.Usuarios
                .AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<int> Insert(Usuario entidad)
        {
            // Validar que el email no exista
            if (await ExisteEmail(entidad.Email))
            {
                throw new Exception("El email ya está registrado");
            }

            await _context.Usuarios.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad.Id;
        }

        public async Task<bool> Update(int id, Usuario entidad)
        {
            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Id == id);

            if (usuarioExistente == null)
                return false;

            // Validar que el email no esté siendo usado por otro usuario
            if (usuarioExistente.Email != entidad.Email && await ExisteEmail(entidad.Email))
            {
                throw new Exception("El email ya está registrado por otro usuario");
            }

            usuarioExistente.Nombre = entidad.Nombre;
            usuarioExistente.Apellido = entidad.Apellido;
            usuarioExistente.Email = entidad.Email;
            usuarioExistente.Telefono = entidad.Telefono;
            usuarioExistente.Direccion = entidad.Direccion;
            usuarioExistente.TipoUsuario = entidad.TipoUsuario;

            // Solo actualizar la contraseña si se proporciona una nueva
            if (!string.IsNullOrEmpty(entidad.Contrasena))
            {
                usuarioExistente.Contrasena = entidad.Contrasena;
            }

            _context.Usuarios.Update(usuarioExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
                return false;

            // Verificar si el usuario tiene carritos activos
            var carritosActivos = await _context.Carritos
                .AnyAsync(c => c.UsuarioId == id && c.Estado == "Activo");

            if (carritosActivos)
            {
                throw new Exception("No se puede eliminar el usuario porque tiene carritos activos");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarTipoUsuario(int id, string tipoUsuario)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
                return false;

            usuario.TipoUsuario = tipoUsuario;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarPerfil(int id, string nombre, string apellido, string telefono, string direccion)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
                return false;

            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Telefono = telefono;
            usuario.Direccion = direccion;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarContrasena(int id, string nuevaContrasena)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
                return false;

            usuario.Contrasena = nuevaContrasena;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario?> ValidarCredenciales(string email, string contrasena)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.Contrasena == contrasena);
        }
    }
}
