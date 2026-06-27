
namespace engin_project.Models
{

    public class ApiResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class CategoriaDocumento
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class ListadoDocumento
    {
        public int IdDocumento { get; set; }
        public int IdCategoria { get; set; }
        public int IdUsuario { get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }        
        public string FechaDocumento { get; set; }
        public string RutaArchivo { get; set; }        
    }

    public class DownloadDocument
    { 
        public int IdDescarga { get; set; }
        public int IdDocumento { get; set; }
        public int IdUsuario { get; set; }

    }
}
