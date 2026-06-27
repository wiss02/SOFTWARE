namespace engin_project.Models
{   
    public class UsuarioListado
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string Rol { get; set; }
        public int IdRol { get; set; }        
    }

    public class Rol
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
    }
}