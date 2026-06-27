using engin_project.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace engin_project.Services
{
    internal class LLMService
    {
        private readonly string apiUrl =
            "http://127.0.0.1:8000/";

        public async Task<ApiResponse> GenerarEmbeddings(string rutaArchivo)
        {
            using (HttpClient client = new HttpClient())
            using (MultipartFormDataContent form = new MultipartFormDataContent())
            {
                byte[] fileBytes = File.ReadAllBytes(rutaArchivo);
                ByteArrayContent fileContent = new ByteArrayContent(fileBytes);

                string extension = Path.GetExtension(rutaArchivo).ToLower();
                string mimeType = "";

                if (extension == ".pdf")
                    mimeType = "application/pdf";
                else if (extension == ".png")
                    mimeType = "image/png";
                else if (extension == ".jpg" || extension == ".jpeg")
                    mimeType = "image/jpeg";
                else
                    return new ApiResponse
                    {
                        success = false,
                        message = "Tipo de archivo no permitido."
                    };

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);
                form.Add(fileContent, "file", Path.GetFileName(rutaArchivo));

                HttpResponseMessage response = await client.PostAsync(apiUrl+ "embeddings-text", form);

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse>(json);
            }
        }

        public async Task<ApiResponse>
            EnviarDocumento(
                string rutaArchivo,
                int idCategoria,
                int idUsuario
            )
        {
            using (HttpClient client = new HttpClient())
            using (MultipartFormDataContent form =
                new MultipartFormDataContent())
            {
                byte[] fileBytes =
                    File.ReadAllBytes(rutaArchivo);

                ByteArrayContent fileContent =
                    new ByteArrayContent(fileBytes);

                string extension =
                    Path.GetExtension(rutaArchivo)
                    .ToLower();

                string mimeType = "";

                if (extension == ".pdf")
                    mimeType = "application/pdf";

                else if (extension == ".png")
                    mimeType = "image/png";

                else if (
                    extension == ".jpg" ||
                    extension == ".jpeg"
                )
                    mimeType = "image/jpeg";

                else
                {
                    return new ApiResponse
                    {
                        success = false,
                        message =
                            "Tipo de archivo no permitido."
                    };
                }

                fileContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse(
                        mimeType
                    );

                form.Add(
                    fileContent,
                    "file",
                    Path.GetFileName(rutaArchivo)
                );

                form.Add(
                    new StringContent(
                        idCategoria.ToString()
                    ),
                    "id_categoria"
                );

                form.Add(
                    new StringContent(
                        idUsuario.ToString()
                    ),
                    "id_usuario"
                );

                form.Add(
                    new StringContent(rutaArchivo),
                    "ruta_archivo"
                );

                HttpResponseMessage response =
                    await client.PostAsync(
                        apiUrl+ "extract-text",
                        form
                    );

                string json =
                    await response.Content
                    .ReadAsStringAsync();

                return JsonSerializer.Deserialize
                    <ApiResponse>(json);
            }
        }
    }
}