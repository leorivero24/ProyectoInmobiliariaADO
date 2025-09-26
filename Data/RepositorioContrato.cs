// using MySql.Data.MySqlClient;
// using ProyectoInmobiliariaADO.Models;
// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace ProyectoInmobiliariaADO.Data
// {
//     public class RepositorioContrato
//     {
//         private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

//         // --- Obtener todos los contratos ---
//         public List<Contrato> ObtenerTodos()
//         {
//             var lista = new List<Contrato>();
//             using var conn = new MySqlConnection(connectionString);
//             string sql = @"
//                 SELECT c.Id, c.InmuebleId, i.Direccion AS InmuebleDireccion, t.Descripcion AS InmuebleTipo,
//                        c.InquilinoId, q.Nombre AS InquilinoNombre, q.Apellido AS InquilinoApellido, q.DNI AS InquilinoDNI,
//                        c.FechaInicio, c.FechaFin, c.Monto, c.Estado, c.Observaciones, c.Periodicidad,
//                        c.UsuarioCreacionContrato, c.UsuarioAnulacionContrato, c.FechaAnulacionContrato
//                 FROM contrato c
//                 INNER JOIN inmueble i ON c.InmuebleId = i.Id
//                 INNER JOIN tipoinmueble t ON i.TipoId = t.Id
//                 INNER JOIN inquilino q ON c.InquilinoId = q.Id";
//             using var cmd = new MySqlCommand(sql, conn);
//             conn.Open();
//             using var r = cmd.ExecuteReader();
//             while (r.Read())
//             {
//                 lista.Add(MapearContrato(r));
//             }
//             return lista;
//         }

//         // --- Obtener por ID ---
//         public Contrato? ObtenerPorId(int id)
//         {
//             using var conn = new MySqlConnection(connectionString);
//             string sql = @"
//                 SELECT c.Id, c.InmuebleId, i.Direccion AS InmuebleDireccion, t.Descripcion AS InmuebleTipo,
//                        c.InquilinoId, q.Nombre AS InquilinoNombre, q.Apellido AS InquilinoApellido, q.DNI AS InquilinoDNI,
//                        c.FechaInicio, c.FechaFin, c.Monto, c.Estado, c.Observaciones, c.Periodicidad,
//                        c.UsuarioCreacionContrato, c.UsuarioAnulacionContrato, c.FechaAnulacionContrato
//                 FROM contrato c
//                 INNER JOIN inmueble i ON c.InmuebleId = i.Id
//                 INNER JOIN tipoinmueble t ON i.TipoId = t.Id
//                 INNER JOIN inquilino q ON c.InquilinoId = q.Id
//                 WHERE c.Id=@id";
//             using var cmd = new MySqlCommand(sql, conn);
//             cmd.Parameters.AddWithValue("@id", id);
//             conn.Open();
//             using var r = cmd.ExecuteReader();
//             if (r.Read()) return MapearContrato(r);
//             return null;
//         }

//         // --- Obtener contratos de un inmueble ---
//         public List<Contrato> ObtenerContratosPorInmueble(int inmuebleId)
//         {
//             var lista = new List<Contrato>();
//             using var conn = new MySqlConnection(connectionString);
//             string sql = @"
//                 SELECT c.Id, c.InmuebleId, i.Direccion AS InmuebleDireccion, t.Descripcion AS InmuebleTipo,
//                        c.InquilinoId, q.Nombre AS InquilinoNombre, q.Apellido AS InquilinoApellido, q.DNI AS InquilinoDNI,
//                        c.FechaInicio, c.FechaFin, c.Monto, c.Estado, c.Observaciones, c.Periodicidad,
//                        c.UsuarioCreacionContrato, c.UsuarioAnulacionContrato, c.FechaAnulacionContrato
//                 FROM contrato c
//                 INNER JOIN inmueble i ON c.InmuebleId = i.Id
//                 INNER JOIN tipoinmueble t ON i.TipoId = t.Id
//                 INNER JOIN inquilino q ON c.InquilinoId = q.Id
//                 WHERE c.InmuebleId=@InmuebleId";
//             using var cmd = new MySqlCommand(sql, conn);
//             cmd.Parameters.AddWithValue("@InmuebleId", inmuebleId);
//             conn.Open();
//             using var r = cmd.ExecuteReader();
//             while (r.Read()) lista.Add(MapearContrato(r));
//             return lista;
//         }

//         // --- Validación de fechas ---
//         public bool FechasDisponibles(int inmuebleId, DateTime fechaInicio, DateTime? fechaFin, int? contratoIdExcluido = null)
//         {
//             using var conn = new MySqlConnection(connectionString);
//             string sql = @"
//                 SELECT COUNT(*) 
//                 FROM contrato 
//                 WHERE InmuebleId=@InmuebleId 
//                   AND Estado='Vigente'
//                   AND (@FechaFin IS NULL OR FechaFin >= @FechaInicio)
//                   AND FechaInicio <= IFNULL(@FechaFin, FechaInicio)";
//             if (contratoIdExcluido.HasValue) sql += " AND Id <> @ContratoId";

//             using var cmd = new MySqlCommand(sql, conn);
//             cmd.Parameters.AddWithValue("@InmuebleId", inmuebleId);
//             cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
//             cmd.Parameters.AddWithValue("@FechaFin", (object?)fechaFin ?? DBNull.Value);
//             if (contratoIdExcluido.HasValue) cmd.Parameters.AddWithValue("@ContratoId", contratoIdExcluido.Value);
//             conn.Open();
//             return Convert.ToInt32(cmd.ExecuteScalar()) == 0;
//         }

//         // --- Alta con auditoría ---
//         public int Alta(Contrato c, string usuario)
//         {
//             if (!FechasDisponibles(c.InmuebleId, c.FechaInicio, c.FechaFin))
//                 throw new Exception("El inmueble ya tiene un contrato vigente en esas fechas.");

//             int res = 0;
//             using var conn = new MySqlConnection(connectionString);
//             conn.Open();
//             using var tx = conn.BeginTransaction();
//             try
//             {
//                 string sqlInsert = @"INSERT INTO contrato 
//                     (InmuebleId, InquilinoId, FechaInicio, FechaFin, Monto, Estado, Observaciones, Periodicidad, UsuarioCreacionContrato)
//                     VALUES (@InmuebleId, @InquilinoId, @FechaInicio, @FechaFin, @Monto, @Estado, @Observaciones, @Periodicidad, @Usuario)";
//                 using var cmd = new MySqlCommand(sqlInsert, conn, tx);
//                 cmd.Parameters.AddWithValue("@InmuebleId", c.InmuebleId);
//                 cmd.Parameters.AddWithValue("@InquilinoId", c.InquilinoId);
//                 cmd.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
//                 cmd.Parameters.AddWithValue("@FechaFin", (object?)c.FechaFin ?? DBNull.Value);
//                 cmd.Parameters.AddWithValue("@Monto", c.Monto);
//                 cmd.Parameters.AddWithValue("@Estado", c.Estado ?? "Vigente");
//                 cmd.Parameters.AddWithValue("@Observaciones", (object?)c.Observaciones ?? DBNull.Value);
//                 cmd.Parameters.AddWithValue("@Periodicidad", c.Periodicidad ?? "Mensual");
//                 cmd.Parameters.AddWithValue("@Usuario", usuario);

//                 res += cmd.ExecuteNonQuery();

//                 ActualizarEstadoInmueble(c.InmuebleId, conn, tx);

//                 tx.Commit();
//             }
//             catch
//             {
//                 tx.Rollback();
//                 throw;
//             }
//             return res;
//         }

//         // --- Modificación ---
//         public int Modificacion(Contrato c)
//         {
//             if (!FechasDisponibles(c.InmuebleId, c.FechaInicio, c.FechaFin, c.Id))
//                 throw new Exception("El inmueble ya tiene un contrato vigente en esas fechas.");

//             int res = 0;
//             using var conn = new MySqlConnection(connectionString);
//             conn.Open();
//             using var tx = conn.BeginTransaction();
//             try
//             {
//                 string sql = @"UPDATE contrato SET 
//                     InmuebleId=@InmuebleId, InquilinoId=@InquilinoId, FechaInicio=@FechaInicio, FechaFin=@FechaFin,
//                     Monto=@Monto, Estado=@Estado, Observaciones=@Observaciones, Periodicidad=@Periodicidad
//                     WHERE Id=@Id";
//                 using var cmd = new MySqlCommand(sql, conn, tx);
//                 cmd.Parameters.AddWithValue("@InmuebleId", c.InmuebleId);
//                 cmd.Parameters.AddWithValue("@InquilinoId", c.InquilinoId);
//                 cmd.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
//                 cmd.Parameters.AddWithValue("@FechaFin", (object?)c.FechaFin ?? DBNull.Value);
//                 cmd.Parameters.AddWithValue("@Monto", c.Monto);
//                 cmd.Parameters.AddWithValue("@Estado", c.Estado ?? "Vigente");
//                 cmd.Parameters.AddWithValue("@Observaciones", (object?)c.Observaciones ?? DBNull.Value);
//                 cmd.Parameters.AddWithValue("@Periodicidad", c.Periodicidad ?? "Mensual");
//                 cmd.Parameters.AddWithValue("@Id", c.Id);
//                 res += cmd.ExecuteNonQuery();

//                 ActualizarEstadoInmueble(c.InmuebleId, conn, tx);

//                 tx.Commit();
//             }
//             catch
//             {
//                 tx.Rollback();
//                 throw;
//             }
//             return res;
//         }

//         // --- Anulación con auditoría ---
//         public int Anular(int id, string usuario)
//         {
//             int res = 0;
//             using var conn = new MySqlConnection(connectionString);
//             conn.Open();
//             using var tx = conn.BeginTransaction();
//             try
//             {
//                 string sql = @"UPDATE contrato SET 
//                                 Estado='Anulado', 
//                                 UsuarioAnulacionContrato=@Usuario, 
//                                 FechaAnulacionContrato=@Fecha
//                                WHERE Id=@Id";
//                 using var cmd = new MySqlCommand(sql, conn, tx);
//                 cmd.Parameters.AddWithValue("@Id", id);
//                 cmd.Parameters.AddWithValue("@Usuario", usuario);
//                 cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
//                 res += cmd.ExecuteNonQuery();

//                 string sqlInm = "SELECT InmuebleId FROM contrato WHERE Id=@Id";
//                 using var cmd2 = new MySqlCommand(sqlInm, conn, tx);
//                 cmd2.Parameters.AddWithValue("@Id", id);
//                 int inmuebleId = Convert.ToInt32(cmd2.ExecuteScalar());

//                 ActualizarEstadoInmueble(inmuebleId, conn, tx);

//                 tx.Commit();
//             }
//             catch
//             {
//                 tx.Rollback();
//                 throw;
//             }
//             return res;
//         }

//         // --- Renovación ---
//         public int Renovar(Contrato c, DateTime nuevaFechaInicio, DateTime? nuevaFechaFin, decimal nuevoMonto, string usuario)
//         {
//             if (!FechasDisponibles(c.InmuebleId, nuevaFechaInicio, nuevaFechaFin))
//                 throw new Exception("El inmueble ya tiene un contrato vigente en esas fechas.");

//             var nuevoContrato = new Contrato
//             {
//                 InmuebleId = c.InmuebleId,
//                 InquilinoId = c.InquilinoId,
//                 FechaInicio = nuevaFechaInicio,
//                 FechaFin = nuevaFechaFin,
//                 Monto = nuevoMonto,
//                 Estado = "Vigente",
//                 Observaciones = "Renovación de contrato ID " + c.Id,
//                 Periodicidad = c.Periodicidad
//             };

//             return Alta(nuevoContrato, usuario);
//         }

//         // --- Mapeo ---
//         private Contrato MapearContrato(MySqlDataReader r)
//         {
//             return new Contrato
//             {
//                 Id = r.GetInt32("Id"),
//                 InmuebleId = r.GetInt32("InmuebleId"),
//                 InmuebleDireccion = r.GetString("InmuebleDireccion"),
//                 InmuebleTipo = r.GetString("InmuebleTipo"),
//                 InquilinoId = r.GetInt32("InquilinoId"),
//                 InquilinoNombre = r.GetString("InquilinoNombre"),
//                 InquilinoApellido = r.GetString("InquilinoApellido"),
//                 InquilinoDNI = r.GetString("InquilinoDNI"),
//                 FechaInicio = r.GetDateTime("FechaInicio"),
//                 FechaFin = r.IsDBNull(r.GetOrdinal("FechaFin")) ? (DateTime?)null : r.GetDateTime("FechaFin"),
//                 Monto = r.GetDecimal("Monto"),
//                 Estado = r.GetString("Estado"),
//                 Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones"),
//                 Periodicidad = r.IsDBNull(r.GetOrdinal("Periodicidad")) ? "Mensual" : r.GetString("Periodicidad"),
//                 UsuarioCreacionContrato = r.IsDBNull(r.GetOrdinal("UsuarioCreacionContrato")) ? null : r.GetString("UsuarioCreacionContrato"),
//                 UsuarioAnulacionContrato = r.IsDBNull(r.GetOrdinal("UsuarioAnulacionContrato")) ? null : r.GetString("UsuarioAnulacionContrato"),
//                 FechaAnulacionContrato = r.IsDBNull(r.GetOrdinal("FechaAnulacionContrato")) ? (DateTime?)null : r.GetDateTime("FechaAnulacionContrato")
//             };
//         }

//         // --- Actualizar Estado del Inmueble ---
//         private void ActualizarEstadoInmueble(int inmuebleId, MySqlConnection conn, MySqlTransaction tx)
//         {
//             string sql = "SELECT COUNT(*) FROM contrato WHERE InmuebleId=@InmuebleId AND Estado='Vigente'";
//             using var cmd = new MySqlCommand(sql, conn, tx);
//             cmd.Parameters.AddWithValue("@InmuebleId", inmuebleId);
//             int count = Convert.ToInt32(cmd.ExecuteScalar());

//             string nuevoEstado = count > 0 ? "Alquilado" : "Disponible";

//             string sqlUpd = "UPDATE inmueble SET Estado=@Estado WHERE Id=@InmuebleId";
//             using var cmd2 = new MySqlCommand(sqlUpd, conn, tx);
//             cmd2.Parameters.AddWithValue("@Estado", nuevoEstado);
//             cmd2.Parameters.AddWithValue("@InmuebleId", inmuebleId);
//             cmd2.ExecuteNonQuery();
//         }

//         // --- Eliminar definitivo ---
//         public void EliminarDefinitivo(int id)
//         {
//             using var conn = new MySqlConnection(connectionString);
//             conn.Open();
//             using var tx = conn.BeginTransaction();
//             try
//             {
//                 string sqlInmueble = "SELECT InmuebleId FROM contrato WHERE Id=@Id";
//                 using var cmd1 = new MySqlCommand(sqlInmueble, conn, tx);
//                 cmd1.Parameters.AddWithValue("@Id", id);
//                 int inmuebleId = Convert.ToInt32(cmd1.ExecuteScalar());

//                 string sqlDelete = "DELETE FROM contrato WHERE Id=@Id";
//                 using var cmd2 = new MySqlCommand(sqlDelete, conn, tx);
//                 cmd2.Parameters.AddWithValue("@Id", id);
//                 cmd2.ExecuteNonQuery();

//                 ActualizarEstadoInmueble(inmuebleId, conn, tx);
//                 tx.Commit();
//             }
//             catch
//             {
//                 tx.Rollback();
//                 throw;
//             }
//         }
//     }
// }



using MySql.Data.MySqlClient;
using ProyectoInmobiliariaADO.Models;
using System;
using System.Collections.Generic;

namespace ProyectoInmobiliariaADO.Data
{
    public class RepositorioContrato
    {
        private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

        // --- Obtener todos los contratos ---
        public List<Contrato> ObtenerTodos()
        {
            var lista = new List<Contrato>();
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
                SELECT c.Id, c.InmuebleId, i.Direccion AS InmuebleDireccion, t.Descripcion AS InmuebleTipo,
                       c.InquilinoId, q.Nombre AS InquilinoNombre, q.Apellido AS InquilinoApellido, q.DNI AS InquilinoDNI,
                       c.FechaInicio, c.FechaFin, c.Monto, c.Estado, c.Observaciones, c.Periodicidad,
                       c.UsuarioCreacionContrato, c.FechaCreacionContrato, c.UsuarioAnulacionContrato, c.FechaAnulacionContrato
                FROM contrato c
                INNER JOIN inmueble i ON c.InmuebleId = i.Id
                INNER JOIN tipoinmueble t ON i.TipoId = t.Id
                INNER JOIN inquilino q ON c.InquilinoId = q.Id";
            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(MapearContrato(r));
            }
            return lista;
        }

        // --- Obtener por ID ---
        public Contrato? ObtenerPorId(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
                SELECT c.Id, c.InmuebleId, i.Direccion AS InmuebleDireccion, t.Descripcion AS InmuebleTipo,
                       c.InquilinoId, q.Nombre AS InquilinoNombre, q.Apellido AS InquilinoApellido, q.DNI AS InquilinoDNI,
                       c.FechaInicio, c.FechaFin, c.Monto, c.Estado, c.Observaciones, c.Periodicidad,
                       c.UsuarioCreacionContrato, c.FechaCreacionContrato, c.UsuarioAnulacionContrato, c.FechaAnulacionContrato
                FROM contrato c
                INNER JOIN inmueble i ON c.InmuebleId = i.Id
                INNER JOIN tipoinmueble t ON i.TipoId = t.Id
                INNER JOIN inquilino q ON c.InquilinoId = q.Id
                WHERE c.Id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (r.Read()) return MapearContrato(r);
            return null;
        }

        // --- Obtener contratos de un inmueble ---
        public List<Contrato> ObtenerContratosPorInmueble(int inmuebleId)
        {
            var lista = new List<Contrato>();
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
                SELECT c.Id, c.InmuebleId, i.Direccion AS InmuebleDireccion, t.Descripcion AS InmuebleTipo,
                       c.InquilinoId, q.Nombre AS InquilinoNombre, q.Apellido AS InquilinoApellido, q.DNI AS InquilinoDNI,
                       c.FechaInicio, c.FechaFin, c.Monto, c.Estado, c.Observaciones, c.Periodicidad,
                       c.UsuarioCreacionContrato, c.FechaCreacionContrato, c.UsuarioAnulacionContrato, c.FechaAnulacionContrato
                FROM contrato c
                INNER JOIN inmueble i ON c.InmuebleId = i.Id
                INNER JOIN tipoinmueble t ON i.TipoId = t.Id
                INNER JOIN inquilino q ON c.InquilinoId = q.Id
                WHERE c.InmuebleId=@InmuebleId";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@InmuebleId", inmuebleId);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read()) lista.Add(MapearContrato(r));
            return lista;
        }

        // --- Validación de fechas ---
        public bool FechasDisponibles(int inmuebleId, DateTime fechaInicio, DateTime? fechaFin, int? contratoIdExcluido = null)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
                SELECT COUNT(*) 
                FROM contrato 
                WHERE InmuebleId=@InmuebleId 
                  AND Estado='Vigente'
                  AND (@FechaFin IS NULL OR FechaFin >= @FechaInicio)
                  AND FechaInicio <= IFNULL(@FechaFin, FechaInicio)";
            if (contratoIdExcluido.HasValue) sql += " AND Id <> @ContratoId";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@InmuebleId", inmuebleId);
            cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", (object?)fechaFin ?? DBNull.Value);
            if (contratoIdExcluido.HasValue) cmd.Parameters.AddWithValue("@ContratoId", contratoIdExcluido.Value);
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) == 0;
        }

        // --- Alta con auditoría ---
        public int Alta(Contrato c, string usuario)
        {
            if (!FechasDisponibles(c.InmuebleId, c.FechaInicio, c.FechaFin))
                throw new Exception("El inmueble ya tiene un contrato vigente en esas fechas.");

            int res = 0;
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                string sqlInsert = @"INSERT INTO contrato 
                    (InmuebleId, InquilinoId, FechaInicio, FechaFin, Monto, Estado, Observaciones, Periodicidad, UsuarioCreacionContrato, FechaCreacionContrato)
                    VALUES (@InmuebleId, @InquilinoId, @FechaInicio, @FechaFin, @Monto, @Estado, @Observaciones, @Periodicidad, @Usuario, @FechaCreacion)";
                using var cmd = new MySqlCommand(sqlInsert, conn, tx);
                cmd.Parameters.AddWithValue("@InmuebleId", c.InmuebleId);
                cmd.Parameters.AddWithValue("@InquilinoId", c.InquilinoId);
                cmd.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", (object?)c.FechaFin ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Monto", c.Monto);
                cmd.Parameters.AddWithValue("@Estado", c.Estado ?? "Vigente");
                cmd.Parameters.AddWithValue("@Observaciones", (object?)c.Observaciones ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Periodicidad", c.Periodicidad ?? "Mensual");
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);

                res += cmd.ExecuteNonQuery();

                ActualizarEstadoInmueble(c.InmuebleId, conn, tx);

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            return res;
        }

        // --- Modificación ---
        public int Modificacion(Contrato c)
        {
            if (!FechasDisponibles(c.InmuebleId, c.FechaInicio, c.FechaFin, c.Id))
                throw new Exception("El inmueble ya tiene un contrato vigente en esas fechas.");

            int res = 0;
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                string sql = @"UPDATE contrato SET 
                    InmuebleId=@InmuebleId, InquilinoId=@InquilinoId, FechaInicio=@FechaInicio, FechaFin=@FechaFin,
                    Monto=@Monto, Estado=@Estado, Observaciones=@Observaciones, Periodicidad=@Periodicidad
                    WHERE Id=@Id";
                using var cmd = new MySqlCommand(sql, conn, tx);
                cmd.Parameters.AddWithValue("@InmuebleId", c.InmuebleId);
                cmd.Parameters.AddWithValue("@InquilinoId", c.InquilinoId);
                cmd.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", (object?)c.FechaFin ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Monto", c.Monto);
                cmd.Parameters.AddWithValue("@Estado", c.Estado ?? "Vigente");
                cmd.Parameters.AddWithValue("@Observaciones", (object?)c.Observaciones ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Periodicidad", c.Periodicidad ?? "Mensual");
                cmd.Parameters.AddWithValue("@Id", c.Id);
                res += cmd.ExecuteNonQuery();

                ActualizarEstadoInmueble(c.InmuebleId, conn, tx);

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            return res;
        }

        // --- Anulación con auditoría ---
        public int Anular(int id, string usuario)
        {
            int res = 0;
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                string sql = @"UPDATE contrato SET 
                                Estado='Anulado', 
                                UsuarioAnulacionContrato=@Usuario, 
                                FechaAnulacionContrato=@Fecha
                               WHERE Id=@Id";
                using var cmd = new MySqlCommand(sql, conn, tx);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                res += cmd.ExecuteNonQuery();

                string sqlInm = "SELECT InmuebleId FROM contrato WHERE Id=@Id";
                using var cmd2 = new MySqlCommand(sqlInm, conn, tx);
                cmd2.Parameters.AddWithValue("@Id", id);
                int inmuebleId = Convert.ToInt32(cmd2.ExecuteScalar());

                ActualizarEstadoInmueble(inmuebleId, conn, tx);

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            return res;
        }

        // --- Renovación ---
        public int Renovar(Contrato c, DateTime nuevaFechaInicio, DateTime? nuevaFechaFin, decimal nuevoMonto, string usuario)
        {
            if (!FechasDisponibles(c.InmuebleId, nuevaFechaInicio, nuevaFechaFin))
                throw new Exception("El inmueble ya tiene un contrato vigente en esas fechas.");

            var nuevoContrato = new Contrato
            {
                InmuebleId = c.InmuebleId,
                InquilinoId = c.InquilinoId,
                FechaInicio = nuevaFechaInicio,
                FechaFin = nuevaFechaFin,
                Monto = nuevoMonto,
                Estado = "Vigente",
                Observaciones = "Renovación de contrato ID " + c.Id,
                Periodicidad = c.Periodicidad
            };

            return Alta(nuevoContrato, usuario);
        }

        // --- Mapeo ---
        private Contrato MapearContrato(MySqlDataReader r)
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
                Estado = r.GetString("Estado"),
                Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones"),
                Periodicidad = r.IsDBNull(r.GetOrdinal("Periodicidad")) ? "Mensual" : r.GetString("Periodicidad"),
                UsuarioCreacionContrato = r.IsDBNull(r.GetOrdinal("UsuarioCreacionContrato")) ? null : r.GetString("UsuarioCreacionContrato"),
                FechaCreacionContrato = r.IsDBNull(r.GetOrdinal("FechaCreacionContrato")) ? (DateTime?)null : r.GetDateTime("FechaCreacionContrato"),
                UsuarioAnulacionContrato = r.IsDBNull(r.GetOrdinal("UsuarioAnulacionContrato")) ? null : r.GetString("UsuarioAnulacionContrato"),
                FechaAnulacionContrato = r.IsDBNull(r.GetOrdinal("FechaAnulacionContrato")) ? (DateTime?)null : r.GetDateTime("FechaAnulacionContrato")
            };
        }

        // --- Actualizar Estado del Inmueble ---
        private void ActualizarEstadoInmueble(int inmuebleId, MySqlConnection conn, MySqlTransaction tx)
        {
            string sql = "SELECT COUNT(*) FROM contrato WHERE InmuebleId=@InmuebleId AND Estado='Vigente'";
            using var cmd = new MySqlCommand(sql, conn, tx);
            cmd.Parameters.AddWithValue("@InmuebleId", inmuebleId);
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            string nuevoEstado = count > 0 ? "Alquilado" : "Disponible";

            string sqlUpd = "UPDATE inmueble SET Estado=@Estado WHERE Id=@InmuebleId";
            using var cmd2 = new MySqlCommand(sqlUpd, conn, tx);
            cmd2.Parameters.AddWithValue("@Estado", nuevoEstado);
            cmd2.Parameters.AddWithValue("@InmuebleId", inmuebleId);
            cmd2.ExecuteNonQuery();
        }

        // --- Eliminar definitivo ---
        public void EliminarDefinitivo(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                string sqlInmueble = "SELECT InmuebleId FROM contrato WHERE Id=@Id";
                using var cmd1 = new MySqlCommand(sqlInmueble, conn, tx);
                cmd1.Parameters.AddWithValue("@Id", id);
                int inmuebleId = Convert.ToInt32(cmd1.ExecuteScalar());

                string sqlDelete = "DELETE FROM contrato WHERE Id=@Id";
                using var cmd2 = new MySqlCommand(sqlDelete, conn, tx);
                cmd2.Parameters.AddWithValue("@Id", id);
                cmd2.ExecuteNonQuery();

                ActualizarEstadoInmueble(inmuebleId, conn, tx);
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}
