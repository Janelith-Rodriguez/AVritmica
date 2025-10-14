using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;
using System.Text.Json;

namespace AVritmica.Client.Servicios
{
    public interface ICategoriaService
    {
        Task<HttpRespuesta<List<Categoria>>> GetCategorias();
        Task<HttpRespuesta<Categoria>> GetCategoria(int id);
        Task<HttpRespuesta<Categoria>> CreateCategoria(Categoria categoria);
        Task<HttpRespuesta<Categoria>> UpdateCategoria(int id, Categoria categoria);
        Task<HttpRespuesta<bool>> DeleteCategoria(int id);
    }
}