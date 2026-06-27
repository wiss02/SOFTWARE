namespace engin_project.Models
{
    public static class Session
    {
        public static int IdUser { get; set; }
        public static string Usuario { get; set; }
        public static string Rol { get; set; }

        public static void Limpiar()
        {
            IdUser = 0;
            Usuario = null;
            Rol = null;
        }
    }
}
