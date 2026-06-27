using Npgsql;

namespace engin_project.Data
{
    internal class DataConexion
    {
    }

    /*public static class DatabaseConnection
    {
        private static readonly string connectionString =
            "Host=localhost;Port=5432;Database=repo_digital;Username=postgres;Password=123456";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }*/

    public static class DatabaseConnection
    {
        private static readonly string connectionString =
            "Host=dpg-d7t5e0ugkk3c73e08p30-a.oregon-postgres.render.com;" +
            "Port=5432;" +
            "Database=repo_digital;" +
            "Username=repo_digital_user;" +
            "Password=ftzkAPrmujUGLaZzpvEB84EcjffBGRof;" +
            "SSL Mode=Require;" +
            "Trust Server Certificate=true;";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
