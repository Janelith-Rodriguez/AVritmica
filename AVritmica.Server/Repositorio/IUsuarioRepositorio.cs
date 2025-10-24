using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Task<List<Usuario>> Select();
        Task<Usuario?> SelectById(int id);
        Task<Usuario?> SelectByEmail(string email);
        Task<List<Usuario>> SelectByTipoUsuario(string tipoUsuario);
        Task<bool> Existe(int id);
        Task<bool> ExisteEmail(string email);
        Task<int> Insert(Usuario entidad);
        Task<bool> Update(int id, Usuario entidad);
        Task<bool> Delete(int id);
        Task<bool> ActualizarTipoUsuario(int id, string tipoUsuario);
        Task<bool> ActualizarPerfil(int id, string nombre, string apellido, string telefono, string direccion);
        Task<bool> CambiarContrasena(int id, string nuevaContrasena);
        Task<Usuario?> ValidarCredenciales(string email, string contrasena);
    }
}
