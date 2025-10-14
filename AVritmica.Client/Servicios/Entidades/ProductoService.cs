using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;
using AVritmica.Shared.DTO;
using System.Text;
using System.Text.Json;

namespace AVritmica.Client.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _http;
        private readonly string _apiUrl = "api/productos";

        public ProductoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<HttpRespuesta<List<Producto>>> GetProductos()
        {
            return await ProcesarSolicitud<List<Producto>>(() => _http.GetAsync(_apiUrl));
        }

        public async Task<HttpRespuesta<Producto>> GetProducto(int id)
        {
            return await ProcesarSolicitud<Producto>(() => _http.GetAsync($"{_apiUrl}/{id}"));
        }

        public async Task<HttpRespuesta<List<Producto>>> GetProductosPorCategoria(int categoriaId)
        {
            return await ProcesarSolicitud<List<Producto>>(() => _http.GetAsync($"{_apiUrl}/GetByCategoria/{categoriaId}"));
        }

        public async Task<HttpRespuesta<List<Producto>>> GetProductosPorPrecioRange(decimal precioMin, decimal precioMax)
        {
            return await ProcesarSolicitud<List<Producto>>(() => _http.GetAsync($"{_apiUrl}/GetByPrecioRange?precioMin={precioMin}&precioMax={precioMax}"));
        }

        public async Task<HttpRespuesta<int>> CreateProducto(CrearProductoDTO producto)
        {
            return await ProcesarSolicitud<int>(async () =>
            {
                var content = new StringContent(JsonSerializer.Serialize(producto), Encoding.UTF8, "application/json");
                return await _http.PostAsync(_apiUrl, content);
            });
        }

        public async Task<HttpRespuesta<bool>> UpdateProducto(int id, Producto producto)
        {
            return await ProcesarSolicitud<bool>(async () =>
            {
                var content = new StringContent(JsonSerializer.Serialize(producto), Encoding.UTF8, "application/json");
                return await _http.PutAsync($"{_apiUrl}/{id}", content);
            });
        }

        public async Task<HttpRespuesta<bool>> DeleteProducto(int id)
        {
            return await ProcesarSolicitud<bool>(() => _http.DeleteAsync($"{_apiUrl}/{id}"));
        }

        public async Task<HttpRespuesta<bool>> ActualizarStock(int id, int cantidad)
        {
            return await ProcesarSolicitud<bool>(async () =>
            {
                var content = new StringContent(JsonSerializer.Serialize(cantidad), Encoding.UTF8, "application/json");
                return await _http.PostAsync($"{_apiUrl}/actualizar-stock/{id}", content);
            });
        }

        public async Task<HttpRespuesta<bool>> ExisteNombre(string nombre)
        {
            return await ProcesarSolicitud<bool>(() => _http.GetAsync($"{_apiUrl}/existeNombre/{nombre}"));
        }

        private async Task<HttpRespuesta<T>> ProcesarSolicitud<T>(Func<Task<HttpResponseMessage>> solicitud)
        {
            try
            {
                var response = await solicitud();
                return await ProcesarRespuesta<T>(response);
            }
            catch (Exception ex)
            {
                // Crear una respuesta de error simulada
                var errorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                return new HttpRespuesta<T>(default, true, errorResponse);
            }
        }

        private async Task<HttpRespuesta<T>> ProcesarRespuesta<T>(HttpResponseMessage response)
        {
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    // Para respuestas sin contenido (NoContent)
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        if (typeof(T) == typeof(bool))
                        {
                            return new HttpRespuesta<T>((T)(object)true, false, response);
                        }
                        return new HttpRespuesta<T>(default, false, response);
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content))
                    {
                        return new HttpRespuesta<T>(default, false, response);
                    }

                    try
                    {
                        var respuesta = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                        return new HttpRespuesta<T>(respuesta, false, response);
                    }
                    catch (JsonException jsonEx)
                    {
                        // Si es un string simple (como un ID), intentar convertir
                        if (typeof(T) == typeof(int) && int.TryParse(content, out int idResult))
                        {
                            return new HttpRespuesta<T>((T)(object)idResult, false, response);
                        }
                        if (typeof(T) == typeof(bool) && bool.TryParse(content, out bool boolResult))
                        {
                            return new HttpRespuesta<T>((T)(object)boolResult, false, response);
                        }

                        return new HttpRespuesta<T>(default, true, response);
                    }
                }
                else
                {
                    return new HttpRespuesta<T>(default, true, response);
                }
            }
            catch (Exception ex)
            {
                return new HttpRespuesta<T>(default, true, response);
            }
        }
    }
}