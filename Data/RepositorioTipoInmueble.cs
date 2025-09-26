using ProyectoInmobiliariaADO.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ProyectoInmobiliariaADO.Data
{
    public class RepositorioTipoInmueble
    {
         private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

        // Obtener todos los tipos activos
        public List<TipoInmueble> ObtenerTodosActivos()
        {
            var lista = new List<TipoInmueble>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = "SELECT Id, Descripcion, Estado, FechaCreacion, FechaModificacion FROM TipoInmueble WHERE Estado = 'Activo'";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new TipoInmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Descripcion = reader.GetString("Descripcion"),
                            Estado = reader.GetString("Estado"),
                            FechaCreacion = reader.GetDateTime("FechaCreacion"),
                            FechaModificacion = reader.GetDateTime("FechaModificacion")
                        });
                    }
                }
            }
            return lista;
        }

        // Obtener por Id
        public TipoInmueble ObtenerPorId(int id)
        {
            TipoInmueble tipo = null;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = "SELECT Id, Descripcion, Estado, FechaCreacion, FechaModificacion FROM TipoInmueble WHERE Id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32("Id"),
                                Descripcion = reader.GetString("Descripcion"),
                                Estado = reader.GetString("Estado"),
                                FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                FechaModificacion = reader.GetDateTime("FechaModificacion")
                            };
                        }
                    }
                }
            }

            return tipo;
        }

        // Alta
        public void Alta(TipoInmueble tipo)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"INSERT INTO TipoInmueble (Descripcion, Estado, FechaCreacion, FechaModificacion)
                              VALUES (@descripcion, 'Activo', NOW(), NOW())";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@descripcion", tipo.Descripcion);
                    cmd.ExecuteNonQuery();
                    tipo.Id = (int)cmd.LastInsertedId;
                }
            }
        }

        // Modificación
        public void Modificacion(TipoInmueble tipo)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"UPDATE TipoInmueble
                              SET Descripcion = @descripcion, FechaModificacion = NOW()
                              WHERE Id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@descripcion", tipo.Descripcion);
                    cmd.Parameters.AddWithValue("@id", tipo.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Baja lógica
        public void Baja(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"UPDATE TipoInmueble
                              SET Estado = 'Inactivo', FechaModificacion = NOW()
                              WHERE Id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
