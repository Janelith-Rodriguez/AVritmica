using AVritmica.BD.Data.Entity;
using AVritmica.Client.Servicios;
using System.Text;
using System.Text.Json;
namespace AVritmica.Client.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient _http;
        private readonly string _apiUrl = "api/categorias";

        public CategoriaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<HttpRespuesta<List<Categoria>>> GetCategorias()
        {
            try
            {
                var response = await _http.GetAsync(_apiUrl);
                return await ProcesarRespuesta<List<Categoria>>(response);
            }
            catch (Exception ex)
            {
                return new HttpRespuesta<List<Categoria>>(default, true, new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public async Task<HttpRespuesta<Categoria>> GetCategoria(int id)
        {
            try
            {
                var response = await _http.GetAsync($"{_apiUrl}/{id}");
                return await ProcesarRespuesta<Categoria>(response);
            }
            catch (Exception ex)
            {
                return new HttpRespuesta<Categoria>(default, true, new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public async Task<HttpRespuesta<Categoria>> CreateCategoria(Categoria categoria)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(categoria), Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(_apiUrl, content);
                return await ProcesarRespuesta<Categoria>(response);
            }
            catch (Exception ex)
            {
                return new HttpRespuesta<Categoria>(default, true, new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public async Task<HttpRespuesta<Categoria>> UpdateCategoria(int id, Categoria categoria)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(categoria), Encoding.UTF8, "application/json");
                var response = await _http.PutAsync($"{_apiUrl}/{id}", content);

                // Para PUT, puede devolver NoContent (204) sin cuerpo
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new HttpRespuesta<Categoria>(categoria, false, response);
                }

                return await ProcesarRespuesta<Categoria>(response);
            }
            catch (Exception ex)
            {
                return new HttpRespuesta<Categoria>(default, true, new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public async Task<HttpRespuesta<bool>> DeleteCategoria(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{_apiUrl}/{id}");

                // Para DELETE, puede devolver NoContent (204) sin cuerpo
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent ||
                    response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new HttpRespuesta<bool>(true, false, response);
                }

                return await ProcesarRespuesta<bool>(response);
            }
            catch (Exception ex)
            {
                return new HttpRespuesta<bool>(false, true, new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        private async Task<HttpRespuesta<T>> ProcesarRespuesta<T>(HttpResponseMessage response)
        {
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    // Si la respuesta es NoContent (204), devolver default
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return new HttpRespuesta<T>(default, false, response);
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    // Si el contenido está vacío, devolver default
                    if (string.IsNullOrEmpty(content))
                    {
                        return new HttpRespuesta<T>(default, false, response);
                    }

                    try
                    {
                        var respuesta = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return new HttpRespuesta<T>(respuesta, false, response);
                    }
                    catch (JsonException)
                    {
                        // Si no se puede deserializar, devolver default
                        return new HttpRespuesta<T>(default, false, response);
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