using AVritmica.BD.Data.Entity;
using System.Text.Json;

namespace AVritmica.Client.Servicios
{
    public class DebugService : IDebugService
    {
        private readonly HttpClient _http;

        public DebugService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> DebugProductos()
        {
            var debugInfo = "=== DEBUG PRODUCTOS ===\n\n";

            try
            {
                // 1. Probar conexión básica
                debugInfo += "1. Probando conexión básica...\n";
                var baseResponse = await _http.GetAsync("");
                debugInfo += $"   Base URL Status: {baseResponse.StatusCode}\n\n";

                // 2. Probar endpoint de productos
                debugInfo += "2. Probando endpoint de productos...\n";
                var response = await _http.GetAsync("api/productos");
                debugInfo += $"   Status Code: {response.StatusCode}\n";
                debugInfo += $"   IsSuccess: {response.IsSuccessStatusCode}\n";
                debugInfo += $"   Content-Type: {response.Content.Headers.ContentType?.MediaType}\n\n";

                // 3. Leer contenido
                var content = await response.Content.ReadAsStringAsync();
                debugInfo += "3. Contenido de la respuesta:\n";
                debugInfo += $"   Longitud: {content.Length} caracteres\n";

                if (!string.IsNullOrEmpty(content))
                {
                    if (content.Length > 1000)
                    {
                        debugInfo += $"   Contenido (primeros 1000 chars):\n{content.Substring(0, 1000)}...\n\n";
                    }
                    else
                    {
                        debugInfo += $"   Contenido:\n{content}\n\n";
                    }

                    // 4. Intentar deserializar
                    debugInfo += "4. Intentando deserializar JSON...\n";
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var productos = JsonSerializer.Deserialize<List<Producto>>(content, options);
                        if (productos != null)
                        {
                            debugInfo += $"   ✅ Deserialización exitosa\n";
                            debugInfo += $"   Cantidad de productos: {productos.Count}\n";

                            if (productos.Count > 0)
                            {
                                debugInfo += $"   Primer producto: {productos[0].Nombre} (ID: {productos[0].Id})\n";
                                debugInfo += $"   Categoría: {productos[0].Categoria?.Nombre ?? "N/A"}\n";
                            }
                        }
                        else
                        {
                            debugInfo += $"   ❌ Deserialización retornó null\n";
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        debugInfo += $"   ❌ Error de deserialización JSON: {jsonEx.Message}\n";
                        debugInfo += $"   Posición: {jsonEx.BytePositionInLine}\n";
                    }
                }
                else
                {
                    debugInfo += "   ❌ Contenido vacío o nulo\n\n";
                }

                // 5. Probar endpoint de categorías
                debugInfo += "\n5. Probando endpoint de categorías...\n";
                var catResponse = await _http.GetAsync("api/categorias");
                debugInfo += $"   Status Code: {catResponse.StatusCode}\n";

                var catContent = await catResponse.Content.ReadAsStringAsync();
                debugInfo += $"   Longitud: {catContent.Length} caracteres\n";
                debugInfo += $"   Contenido categorías: {catContent}\n";
                // 6. Información del HttpClient
                debugInfo += $"\n6. Información del HttpClient:\n";
                debugInfo += $"   BaseAddress: {_http.BaseAddress}\n";
                debugInfo += $"   Timeout: {_http.Timeout}\n";

            }
            catch (HttpRequestException httpEx)
            {
                debugInfo += $"❌ Error de HTTP Request: {httpEx.Message}\n";
                if (httpEx.InnerException != null)
                {
                    debugInfo += $"   Inner Exception: {httpEx.InnerException.Message}\n";
                }
            }
            catch (Exception ex)
            {
                debugInfo += $"❌ Error inesperado: {ex.Message}\n";
                debugInfo += $"   StackTrace: {ex.StackTrace}\n";
            }

            debugInfo += $"\n=== FIN DEBUG ===\n";
            debugInfo += $"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

            return debugInfo;
        }
    }
}
