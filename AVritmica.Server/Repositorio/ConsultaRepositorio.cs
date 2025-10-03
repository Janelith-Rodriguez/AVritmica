using AVritmica.BD.Data;
using AVritmica.BD.Data.Entity;

namespace AVritmica.Server.Repositorio
{
    public class ConsultaRepositorio : Repositorio<Consulta>, IConsultaRepositorio
    {
        private readonly Context context;
        public ConsultaRepositorio(Context context) : base(context)
        {
            this.context = context;
        }
    }
}
