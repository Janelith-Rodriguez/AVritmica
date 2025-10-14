using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;

namespace AVritmica.Client.Servicios.Entidades
{
    public interface IConsultaServicio
    {
        Task<HttpRespuesta<List<Consulta>>> Get();
        Task<HttpRespuesta<Consulta>> Get(int id);
        Task<HttpRespuesta<List<Consulta>>> GetByUsuario(int usuarioId);
        Task<HttpRespuesta<List<Consulta>>> GetByEmail(string email);
        Task<HttpRespuesta<object>> Post(Consulta entidad);
        Task<HttpRespuesta<object>> Put(Consulta entidad);
        Task<HttpRespuesta<object>> Delete(int id);
        Task<HttpRespuesta<object>> EnviarConsulta(Consulta consulta);
    }
}