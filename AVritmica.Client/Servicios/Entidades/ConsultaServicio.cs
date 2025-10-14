using AVritmica.BD.Data.Entity;

namespace AVritmica.Client.Servicios.Entidades
{
    public class ConsultaServicio : IConsultaServicio
    {
        private readonly IHttpServicio http;
        private string url = "api/Consultas";

        public ConsultaServicio(IHttpServicio http) => this.http = http;

        public async Task<HttpRespuesta<List<Consulta>>> Get() => await http.Get<List<Consulta>>(url);
        public async Task<HttpRespuesta<Consulta>> Get(int id) => await http.Get<Consulta>($"{url}/{id}");
        public async Task<HttpRespuesta<List<Consulta>>> GetByUsuario(int usuarioId) => await http.Get<List<Consulta>>($"{url}/GetByUsuario/{usuarioId}");
        public async Task<HttpRespuesta<List<Consulta>>> GetByEmail(string email) => await http.Get<List<Consulta>>($"{url}/GetByEmail/{email}");
        public async Task<HttpRespuesta<object>> Post(Consulta entidad) => await http.Post(url, entidad);
        public async Task<HttpRespuesta<object>> Put(Consulta entidad) => await http.Put($"{url}/{entidad.Id}", entidad);
        public async Task<HttpRespuesta<object>> Delete(int id) => await http.Delete($"{url}/{id}");
        public async Task<HttpRespuesta<object>> EnviarConsulta(Consulta consulta) => await http.Post($"{url}/enviar-consulta", consulta);
    }
}
