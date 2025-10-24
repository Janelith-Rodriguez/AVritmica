using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public interface IConsultaRepositorio
    {
        Task<List<Consulta>> Select();
        Task<Consulta?> SelectById(int id);
        Task<List<Consulta>> SelectByUsuario(int usuarioId);
        Task<List<Consulta>> SelectByEmail(string email);
        Task<List<Consulta>> SelectByRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<List<Consulta>> SelectNoLeidas();
        Task<bool> Existe(int id);
        Task<int> Insert(Consulta entidad);
        Task<int> InsertConsultaPublica(string nombre, string email, string mensaje); // NUEVO MÉTODO
        Task<bool> Update(int id, Consulta entidad);
        Task<bool> Delete(int id);
        Task<bool> MarcarComoLeida(int id);
        Task<int> ObtenerCantidadNoLeidas();
    }
}