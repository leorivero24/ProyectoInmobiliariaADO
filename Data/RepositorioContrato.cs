


using MySql.Data.MySqlClient;
using ProyectoInmobiliariaADO.Models;
using System.Collections.Generic;

namespace ProyectoInmobiliariaADO.Data
{
    public class RepositorioContrato
    {
        private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

        public List<Contrato> ObtenerTodos()
        {
            var lista = new List<Contrato>();
            using var conn = new MySqlConnection(connectionString);

            string sql = @"SELECT c.Id, 
                                  c.InmuebleId, 
                                  i.Direccion AS InmuebleDireccion,
                                  i.Tipo AS InmuebleTipo,
                                  c.InquilinoId, 
                                  q.Nombre AS InquilinoNombre,
                                  q.Apellido AS InquilinoApellido,
                                  q.DNI AS InquilinoDNI,
                                  c.FechaInicio, 
                                  c.FechaFin, 
                                  c.Monto, 
                                  c.Periodicidad, 
                                  c.Estado, 
                                  c.Observaciones
                           FROM contrato c
                           INNER JOIN inmueble i ON c.InmuebleId = i.Id
                           INNER JOIN inquilino q ON c.InquilinoId = q.Id";

            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Contrato
                {
                    Id = r.GetInt32("Id"),
                    InmuebleId = r.GetInt32("InmuebleId"),
                    InmuebleDireccion = r.GetString("InmuebleDireccion"),
                    InmuebleTipo = r.GetString("InmuebleTipo"),
                    InquilinoId = r.GetInt32("InquilinoId"),
                    InquilinoNombre = r.GetString("InquilinoNombre"),
                    InquilinoApellido = r.GetString("InquilinoApellido"),
                    InquilinoDNI = r.GetString("InquilinoDNI"),
                    FechaInicio = r.GetDateTime("FechaInicio"),
                    FechaFin = r.IsDBNull(r.GetOrdinal("FechaFin")) ? (DateTime?)null : r.GetDateTime("FechaFin"),
                    Monto = r.GetDecimal("Monto"),
                    Periodicidad = r.GetString("Periodicidad"),
                    Estado = r.GetString("Estado"),
                    Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones")
                });
            }
            return lista;
        }

        public Contrato? ObtenerPorId(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"SELECT c.Id, 
                                  c.InmuebleId, 
                                  i.Direccion AS InmuebleDireccion,
                                  i.Tipo AS InmuebleTipo,
                                  c.InquilinoId, 
                                  q.Nombre AS InquilinoNombre,
                                  q.Apellido AS InquilinoApellido,
                                  q.DNI AS InquilinoDNI,
                                  c.FechaInicio, 
                                  c.FechaFin, 
                                  c.Monto, 
                                  c.Periodicidad, 
                                  c.Estado, 
                                  c.Observaciones
                           FROM contrato c
                           INNER JOIN inmueble i ON c.InmuebleId = i.Id
                           INNER JOIN inquilino q ON c.InquilinoId = q.Id
                           WHERE c.Id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                return new Contrato
                {
                    Id = r.GetInt32("Id"),
                    InmuebleId = r.GetInt32("InmuebleId"),
                    InmuebleDireccion = r.GetString("InmuebleDireccion"),
                    InmuebleTipo = r.GetString("InmuebleTipo"),
                    InquilinoId = r.GetInt32("InquilinoId"),
                    InquilinoNombre = r.GetString("InquilinoNombre"),
                    InquilinoApellido = r.GetString("InquilinoApellido"),
                    InquilinoDNI = r.GetString("InquilinoDNI"),
                    FechaInicio = r.GetDateTime("FechaInicio"),
                    FechaFin = r.IsDBNull(r.GetOrdinal("FechaFin")) ? (DateTime?)null : r.GetDateTime("FechaFin"),
                    Monto = r.GetDecimal("Monto"),
                    Periodicidad = r.GetString("Periodicidad"),
                    Estado = r.GetString("Estado"),
                    Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones")
                };
            }
            return null;
        }

        public int Alta(Contrato c)
        {
            int res = 0;
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                string sqlInsert = @"INSERT INTO contrato (InmuebleId, InquilinoId, FechaInicio, FechaFin, Monto, Periodicidad, Estado, Observaciones)
                                     VALUES (@InmuebleId, @InquilinoId, @FechaInicio, @FechaFin, @Monto, @Periodicidad, @Estado, @Observaciones)";
                using var cmd = new MySqlCommand(sqlInsert, conn, tx);
                cmd.Parameters.AddWithValue("@InmuebleId", c.InmuebleId);
                cmd.Parameters.AddWithValue("@InquilinoId", c.InquilinoId);
                cmd.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", (object?)c.FechaFin ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Monto", c.Monto);
                cmd.Parameters.AddWithValue("@Periodicidad", c.Periodicidad);
                cmd.Parameters.AddWithValue("@Estado", c.Estado);
                cmd.Parameters.AddWithValue("@Observaciones", (object?)c.Observaciones ?? DBNull.Value);
                res += cmd.ExecuteNonQuery();

                // Actualizar inmueble a 'Alquilado'
                string sqlUpd = "UPDATE inmueble SET Estado='Alquilado' WHERE Id=@InmuebleId";
                using var cmd2 = new MySqlCommand(sqlUpd, conn, tx);
                cmd2.Parameters.AddWithValue("@InmuebleId", c.InmuebleId);
                res += cmd2.ExecuteNonQuery();

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            return res;
        }

        public int Modificacion(Contrato c)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"UPDATE contrato SET InmuebleId=@InmuebleId, InquilinoId=@InquilinoId, FechaInicio=@FechaInicio, FechaFin=@FechaFin,
                           Monto=@Monto, Periodicidad=@Periodicidad, Estado=@Estado, Observaciones=@Observaciones WHERE Id=@Id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@InmuebleId", c.InmuebleId);
            cmd.Parameters.AddWithValue("@InquilinoId", c.InquilinoId);
            cmd.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", (object?)c.FechaFin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Monto", c.Monto);
            cmd.Parameters.AddWithValue("@Periodicidad", c.Periodicidad);
            cmd.Parameters.AddWithValue("@Estado", c.Estado);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)c.Observaciones ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", c.Id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Baja(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = "DELETE FROM contrato WHERE Id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public List<Contrato> ObtenerContratosPorInmueble(int inmuebleId)
        {
            var lista = new List<Contrato>();
            using var conn = new MySqlConnection(connectionString);
            string sql = @"SELECT c.Id, 
                                  c.InmuebleId,
                                  i.Direccion AS InmuebleDireccion,
                                  i.Tipo AS InmuebleTipo,
                                  c.InquilinoId,
                                  q.Nombre AS InquilinoNombre,
                                  q.Apellido AS InquilinoApellido,
                                  q.DNI AS InquilinoDNI,
                                  c.FechaInicio, 
                                  c.FechaFin, 
                                  c.Monto, 
                                  c.Periodicidad, 
                                  c.Estado, 
                                  c.Observaciones
                           FROM contrato c
                           INNER JOIN inmueble i ON c.InmuebleId = i.Id
                           INNER JOIN inquilino q ON c.InquilinoId = q.Id
                           WHERE c.InmuebleId=@InmuebleId";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@InmuebleId", inmuebleId);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Contrato
                {
                    Id = r.GetInt32("Id"),
                    InmuebleId = r.GetInt32("InmuebleId"),
                    InmuebleDireccion = r.GetString("InmuebleDireccion"),
                    InmuebleTipo = r.GetString("InmuebleTipo"),
                    InquilinoId = r.GetInt32("InquilinoId"),
                    InquilinoNombre = r.GetString("InquilinoNombre"),
                    InquilinoApellido = r.GetString("InquilinoApellido"),
                    InquilinoDNI = r.GetString("InquilinoDNI"),
                    FechaInicio = r.GetDateTime("FechaInicio"),
                    FechaFin = r.IsDBNull(r.GetOrdinal("FechaFin")) ? (DateTime?)null : r.GetDateTime("FechaFin"),
                    Monto = r.GetDecimal("Monto"),
                    Periodicidad = r.GetString("Periodicidad"),
                    Estado = r.GetString("Estado"),
                    Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones")
                });
            }
            return lista;
        }
    }
}
