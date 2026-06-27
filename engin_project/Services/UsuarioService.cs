using System;
using System.Collections.Generic;
using Npgsql;
using engin_project.Data;
using engin_project.Models;

namespace engin_project.Services
{        
    internal class UsuarioService
    {
        public List<Rol> ObtenerRoles()
        {
            List<Rol> lista = new List<Rol>();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT id_rol, nombre
                        FROM repositorio.roles
                        WHERE estado = TRUE
                        ORDER BY nombre;
                    ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Rol
                            {
                                IdRol = Convert.ToInt32(reader["id_rol"]),
                                Nombre = reader["nombre"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                throw new Exception("Error al obtener roles: " + ex.Message);
            }

            return lista;
        }

        public bool InsertarUsuario(
            int idRol,
            string nombres,
            string apellidos,
            string usuario,
            string password,
            string correo
        )
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                INSERT INTO repositorio.usuarios
                (
                    id_rol,
                    nombres,
                    apellidos,
                    usuario,
                    password_hash,
                    correo,
                    estado,
                    fecha_registro
                )
                VALUES
                (
                    @id_rol,
                    @nombres,
                    @apellidos,
                    @usuario,
                    @password_hash,
                    @correo,
                    TRUE,
                    CURRENT_TIMESTAMP
                );
            ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_rol", idRol);
                        cmd.Parameters.AddWithValue("@nombres", nombres);
                        cmd.Parameters.AddWithValue("@apellidos", apellidos);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@password_hash", password);
                        cmd.Parameters.AddWithValue("@correo", correo);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar usuario: " + ex.Message);
            }
        }

        public bool ActualizarUsuario(
            int idUsuario,
            int idRol,
            string nombres,
            string apellidos,
            string usuario,
            string password,
            string correo
        )
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    UPDATE repositorio.usuarios
                    SET 
                        id_rol = @idRol,
                        nombres = @nombres,
                        apellidos = @apellidos,
                        usuario = @usuario,
                        password_hash = @password,
                        correo = @correo,
                        fecha_actualizacion = NOW()
                    WHERE id_usuario = @idUsuario
                ";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@idRol", idRol);
                    cmd.Parameters.AddWithValue("@nombres", nombres);
                    cmd.Parameters.AddWithValue("@apellidos", apellidos);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@correo", correo);

                    return cmd.ExecuteNonQuery() > 0;
                    
                }
            }
        }

        public bool EliminarUsuario(int idUsuario)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    UPDATE repositorio.usuarios
                    SET 
                        estado = FALSE,
                        fecha_actualizacion = CURRENT_TIMESTAMP
                    WHERE id_usuario = @idUsuario
                ";

                using (var cmd = new Npgsql.NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<UsuarioListado> ListarUsuariosActivos()
        {
            List<UsuarioListado> lista = new List<UsuarioListado>();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    u.id_usuario,
                    u.nombres,
                    u.apellidos,
                    u.usuario,
                    u.correo,
                    u.password_hash,
                    r.nombre AS rol,
                    r.id_rol AS id_rol                 
                FROM repositorio.usuarios u
                INNER JOIN repositorio.roles r 
                    ON r.id_rol = u.id_rol
                WHERE u.estado = TRUE
                ORDER BY u.id_usuario DESC;
            ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new UsuarioListado
                            {
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                IdRol = Convert.ToInt32(reader["id_rol"]),
                                Nombres = reader["nombres"].ToString(),
                                Apellidos = reader["apellidos"].ToString(),
                                Usuario = reader["usuario"].ToString(),
                                Correo = reader["correo"].ToString(),
                                Contrasenia = reader["password_hash"].ToString(),
                                Rol = reader["rol"].ToString()                                
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar usuarios: " + ex.Message);
            }

            return lista;
        }        
    }
}