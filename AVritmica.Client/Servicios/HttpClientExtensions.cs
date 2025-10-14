using System.Text;
using System.Text.Json;
using AVritmica.Client.Servicios;

namespace AVritmica.Client.Servicios
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpRespuesta<TResponse>> Get<T, TResponse>(this HttpClient http, string url)
        {
            var response = await http.GetAsync(url);
            return await ProcessResponse<TResponse>(response);
        }

        public static async Task<HttpRespuesta<TResponse>> Post<T, TResponse>(this HttpClient http, string url, T data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await http.PostAsync(url, content);
            return await ProcessResponse<TResponse>(response);
        }

        public static async Task<HttpRespuesta<TResponse>> Put<T, TResponse>(this HttpClient http, string url, T data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await http.PutAsync(url, content);
            return await ProcessResponse<TResponse>(response);
        }

        public static async Task<HttpRespuesta<TResponse>> Delete<TResponse>(this HttpClient http, string url)
        {
            var response = await http.DeleteAsync(url);
            return await ProcessResponse<TResponse>(response);
        }

        private static async Task<HttpRespuesta<TResponse>> ProcessResponse<TResponse>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var respuesta = await Deserializar<TResponse>(response);
                return new HttpRespuesta<TResponse>(respuesta, false, response);
            }
            else
            {
                return new HttpRespuesta<TResponse>(default, true, response);
            }
        }

        private static async Task<T?> Deserializar<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
