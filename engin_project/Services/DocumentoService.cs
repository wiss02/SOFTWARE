using Npgsql;
using System;
using System.Collections.Generic;
using engin_project.Data;
using engin_project.Models;

namespace engin_project.Services
{
    internal class DocumentoService
    {
        public List<CategoriaDocumento> ObtenerCategorias()
        {
            List<CategoriaDocumento> lista = new List<CategoriaDocumento>();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT id_categoria, nombre, descripcion
                        FROM repositorio.categorias_documento
                        WHERE estado = TRUE
                        ORDER BY nombre;
                    ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new CategoriaDocumento
                            {
                                IdCategoria = Convert.ToInt32(reader["id_categoria"]),
                                Nombre = reader["nombre"].ToString(),
                                Descripcion = reader["descripcion"]?.ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener categorías: " + ex.Message);
            }

            return lista;
        }

        public bool InsertarDocumento(ListadoDocumento doc)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO repositorio.documentos
                        (
                            id_categoria,
                            id_usuario_registro,
                            titulo,
                            descripcion,
                            codigo,
                            fecha_documento,
                            ruta_archivo,
                            estado,
                            fecha_registro
                        )
                        VALUES
                        (
                            @id_categoria,
                            @id_usuario_registro,
                            @titulo,
                            @descripcion,
                            @codigo,
                            @fecha_documento,
                            @ruta_archivo,
                            TRUE,
                            CURRENT_TIMESTAMP
                        );
                    ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_categoria", doc.IdCategoria);
                        cmd.Parameters.AddWithValue("@id_usuario_registro", doc.IdUsuario);
                        cmd.Parameters.AddWithValue("@titulo", doc.Titulo);
                        cmd.Parameters.AddWithValue("@descripcion", (object)doc.Descripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@codigo", (object)doc.Codigo ?? DBNull.Value);

                        cmd.Parameters.AddWithValue(
                            "@fecha_documento",
                            string.IsNullOrWhiteSpace(doc.FechaDocumento)
                                ? DBNull.Value
                                : (object)DateTime.Parse(doc.FechaDocumento)
                        );

                        cmd.Parameters.AddWithValue("@ruta_archivo", doc.RutaArchivo);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar documento: " + ex.Message);
            }
        }

        public bool ActualizarDocumento(int idDocumento, ListadoDocumento doc)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        UPDATE repositorio.documentos
                        SET
                            id_categoria = @id_categoria,
                            titulo = @titulo,
                            descripcion = @descripcion,
                            codigo = @codigo,
                            fecha_documento = @fecha_documento,
                            ruta_archivo = @ruta_archivo,
                            fecha_actualizacion = CURRENT_TIMESTAMP
                        WHERE id_documento = @id_documento
                          AND estado = TRUE;
                    ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_documento", idDocumento);
                        cmd.Parameters.AddWithValue("@id_categoria", doc.IdCategoria);
                        cmd.Parameters.AddWithValue("@titulo", doc.Titulo);
                        cmd.Parameters.AddWithValue("@descripcion", (object)doc.Descripcion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@codigo", (object)doc.Codigo ?? DBNull.Value);

                        cmd.Parameters.AddWithValue(
                            "@fecha_documento",
                            string.IsNullOrWhiteSpace(doc.FechaDocumento)
                                ? DBNull.Value
                                : (object)DateTime.Parse(doc.FechaDocumento)
                        );

                        cmd.Parameters.AddWithValue("@ruta_archivo", doc.RutaArchivo);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar documento: " + ex.Message);
            }
        }

        public bool EliminarDocumento(int idDocumento)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        UPDATE repositorio.documentos
                        SET
                            estado = FALSE,
                            fecha_actualizacion = CURRENT_TIMESTAMP
                        WHERE id_documento = @id_documento;
                    ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_documento", idDocumento);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar documento: " + ex.Message);
            }
        }

        public List<ListadoDocumento> ListarDocumentosActivos()
        {
            List<ListadoDocumento> lista = new List<ListadoDocumento>();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    id_documento,
                    id_categoria,
                    id_usuario_registro,
                    titulo,
                    descripcion,
                    codigo,
                    fecha_documento,
                    ruta_archivo
                FROM repositorio.documentos
                WHERE estado = TRUE
                ORDER BY fecha_registro DESC;
            ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ListadoDocumento
                            {
                                IdDocumento = Convert.ToInt32(reader["id_documento"]),
                                IdCategoria = Convert.ToInt32(reader["id_categoria"]),
                                IdUsuario = Convert.ToInt32(reader["id_usuario_registro"]),
                                Titulo = reader["titulo"].ToString(),
                                Descripcion = reader["descripcion"] == DBNull.Value ? "" : reader["descripcion"].ToString(),
                                Codigo = reader["codigo"] == DBNull.Value ? "" : reader["codigo"].ToString(),
                                FechaDocumento = reader["fecha_documento"] == DBNull.Value
                                    ? ""
                                    : Convert.ToDateTime(reader["fecha_documento"]).ToString("yyyy-MM-dd"),
                                RutaArchivo = reader["ruta_archivo"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar documentos: " + ex.Message);
            }

            return lista;
        }


        public bool DownloadDocumento(DownloadDocument doc)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO repositorio.descargas_documento
                        (
                            id_documento,
                            id_usuario,                            
                            fecha_descarga
                        )
                        VALUES
                        (
                            @id_documento,
                            @id_usuario,                           
                            CURRENT_TIMESTAMP
                        );
                    ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_documento", doc.IdDocumento);
                        cmd.Parameters.AddWithValue("@id_usuario", doc.IdUsuario);                                                                        
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al realizar la descarga del documento: " + ex.Message);
            }
        }
    }
}
