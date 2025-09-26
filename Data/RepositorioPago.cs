
// using MySql.Data.MySqlClient;
// using ProyectoInmobiliariaADO.Models;
// using System;
// using System.Collections.Generic;

// namespace ProyectoInmobiliariaADO.Data
// {
//     public class RepositorioPago
//     {
//         private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

//         // --- Obtener todos los pagos ---
//         public List<Pago> ObtenerTodos()
//         {
//             var lista = new List<Pago>();
//             using var conn = new MySqlConnection(connectionString);

//             string sql = @"
// SELECT 
//     p.Id, p.Fecha, p.Importe, p.MedioPago, p.Observaciones, p.Estado,
//     c.Id AS ContratoId,
//     i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
//     inm.Direccion AS InmuebleDireccion,
//     t.Descripcion AS InmuebleTipo
// FROM pago p
// INNER JOIN contrato c ON p.ContratoId = c.Id
// INNER JOIN inquilino i ON c.InquilinoId = i.Id
// INNER JOIN inmueble inm ON c.InmuebleId = inm.Id
// LEFT JOIN tipoinmueble t ON inm.TipoId = t.Id";

//             using var cmd = new MySqlCommand(sql, conn);
//             conn.Open();
//             using var r = cmd.ExecuteReader();

//             while (r.Read())
//             {
//                 lista.Add(MapearPago(r));
//             }

//             return lista;
//         }

//         // --- Obtener pago por Id ---
//         public Pago? ObtenerPorId(int id)
//         {
//             using var conn = new MySqlConnection(connectionString);

//             string sql = @"
// SELECT 
//     p.Id, p.Fecha, p.Importe, p.MedioPago, p.Observaciones, p.Estado,
//     c.Id AS ContratoId,
//     i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
//     inm.Direccion AS InmuebleDireccion,
//     t.Descripcion AS InmuebleTipo
// FROM pago p
// INNER JOIN contrato c ON p.ContratoId = c.Id
// INNER JOIN inquilino i ON c.InquilinoId = i.Id
// INNER JOIN inmueble inm ON c.InmuebleId = inm.Id
// LEFT JOIN tipoinmueble t ON inm.TipoId = t.Id
// WHERE p.Id=@id";

//             using var cmd = new MySqlCommand(sql, conn);
//             cmd.Parameters.AddWithValue("@id", id);
//             conn.Open();
//             using var r = cmd.ExecuteReader();

//             if (r.Read())
//             {
//                 return MapearPago(r);
//             }

//             return null;
//         }

//         // --- Obtener pagos por contrato ---
//         public List<Pago> ObtenerPorContrato(int contratoId)
//         {
//             var lista = new List<Pago>();
//             using var conn = new MySqlConnection(connectionString);

//             string sql = @"
// SELECT 
//     p.Id, p.Fecha, p.Importe, p.MedioPago, p.Observaciones, p.Estado,
//     c.Id AS ContratoId,
//     i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
//     inm.Direccion AS InmuebleDireccion,
//     t.Descripcion AS InmuebleTipo
// FROM pago p
// INNER JOIN contrato c ON p.ContratoId = c.Id
// INNER JOIN inquilino i ON c.InquilinoId = i.Id
// INNER JOIN inmueble inm ON c.InmuebleId = inm.Id
// LEFT JOIN tipoinmueble t ON inm.TipoId = t.Id
// WHERE p.ContratoId=@ContratoId";

//             using var cmd = new MySqlCommand(sql, conn);
//             cmd.Parameters.AddWithValue("@ContratoId", contratoId);
//             conn.Open();
//             using var r = cmd.ExecuteReader();

//             while (r.Read())
//             {
//                 lista.Add(MapearPago(r));
//             }

//             return lista;
//         }

//         // --- Alta de pago con auditoría ---
//         public int Alta(Pago p, string usuario)
//         {
//             using var conn = new MySqlConnection(connectionString);
//             string sql = @"
// INSERT INTO pago 
//     (ContratoId, Fecha, Importe, MedioPago, Observaciones, Estado, UsuarioCreacionPago, FechaCreacionPago)
// VALUES 
//     (@ContratoId, @Fecha, @Importe, @MedioPago, @Observaciones, @Estado, @Usuario, @FechaCreacion)";

//             using var cmd = new MySqlCommand(sql, conn);
//             cmd.Parameters.AddWithValue("@ContratoId", p.ContratoId);
//             cmd.Parameters.AddWithValue("@Fecha", p.Fecha);
//             cmd.Parameters.AddWithValue("@Importe", p.Importe);
//             cmd.Parameters.AddWithValue("@MedioPago", (object?)p.MedioPago ?? DBNull.Value);
//             cmd.Parameters.AddWithValue("@Observaciones", (object?)p.Observaciones ?? DBNull.Value);
//             cmd.Parameters.AddWithValue("@Estado", p.Estado ?? "Activo");
//             cmd.Parameters.AddWithValue("@Usuario", usuario);
//             cmd.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);

//             conn.Open();
//             return cmd.ExecuteNonQuery();
//         }

//         // --- Modificación de pago ---
//         public int Modificacion(Pago p)
//         {
//             using var conn = new MySqlConnection(connectionString);

//             string sql = @"UPDATE pago 
//                            SET Observaciones=@Observaciones,
//                                Estado=@Estado,
//                                UsuarioAnulacionPago=@UsuarioAnulacion,
//                                FechaAnulacionPago=@FechaAnulacion
//                            WHERE Id=@Id";

//             using var cmd = new MySqlCommand(sql, conn);
//             cmd.Parameters.AddWithValue("@Observaciones", (object?)p.Observaciones ?? DBNull.Value);
//             cmd.Parameters.AddWithValue("@Estado", (object?)p.Estado ?? DBNull.Value);
//             cmd.Parameters.AddWithValue("@UsuarioAnulacion", (object?)p.UsuarioAnulacionPago ?? DBNull.Value);
//             cmd.Parameters.AddWithValue("@FechaAnulacion", (object?)p.FechaAnulacionPago ?? DBNull.Value);
//             cmd.Parameters.AddWithValue("@Id", p.Id);

//             conn.Open();
//             return cmd.ExecuteNonQuery();
//         }

//         // --- Eliminar pago ---
//         public int EliminarDefinitivo(int id)
//         {
//             int res = 0;
//             using var conn = new MySqlConnection(connectionString);
//             conn.Open();
//             using var tx = conn.BeginTransaction();
//             try
//             {
//                 string sql = "DELETE FROM pago WHERE Id=@Id";
//                 using var cmd = new MySqlCommand(sql, conn, tx);
//                 cmd.Parameters.AddWithValue("@Id", id);
//                 res = cmd.ExecuteNonQuery();

//                 tx.Commit();
//             }
//             catch
//             {
//                 tx.Rollback();
//                 throw;
//             }
//             return res;
//         }

//         // --- Mapeo de pago con info de contrato ---
//         private Pago MapearPago(MySqlDataReader r)
//         {
//             return new Pago
//             {
//                 Id = r.GetInt32("Id"),
//                 ContratoId = r.GetInt32("ContratoId"),
//                 Fecha = r.GetDateTime("Fecha"),
//                 Importe = r.GetDecimal("Importe"),
//                 MedioPago = r.IsDBNull(r.GetOrdinal("MedioPago")) ? null : r.GetString("MedioPago"),
//                 Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones"),
//                 Estado = r.IsDBNull(r.GetOrdinal("Estado")) ? "Activo" : r.GetString("Estado"),

//                 ContratoInmuebleDireccion = r.GetString("InmuebleDireccion"),
//                 ContratoInmuebleTipo = r.IsDBNull(r.GetOrdinal("InmuebleTipo")) ? "Sin tipo" : r.GetString("InmuebleTipo"),
//                 ContratoInquilinoNombre = r.GetString("InquilinoNombre"),
//                 ContratoInquilinoApellido = r.GetString("InquilinoApellido")
//             };
//         }
//     }
// }

using MySql.Data.MySqlClient;
using ProyectoInmobiliariaADO.Models;
using System;
using System.Collections.Generic;

namespace ProyectoInmobiliariaADO.Data
{
    public class RepositorioPago
    {
        private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

        // --- Obtener todos los pagos ---
        public List<Pago> ObtenerTodos()
        {
            var lista = new List<Pago>();
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
                SELECT 
                    p.Id, p.ContratoId, p.Fecha, p.Importe, p.MedioPago, p.Observaciones, p.Estado,
                    p.UsuarioCreacionPago, p.FechaCreacionPago,
                    p.UsuarioAnulacionPago, p.FechaAnulacionPago,
                    c.Id AS ContratoId,
                    i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
                    inm.Direccion AS InmuebleDireccion,
                    t.Descripcion AS InmuebleTipo
                FROM pago p
                INNER JOIN contrato c ON p.ContratoId = c.Id
                INNER JOIN inquilino i ON c.InquilinoId = i.Id
                INNER JOIN inmueble inm ON c.InmuebleId = inm.Id
                LEFT JOIN tipoinmueble t ON inm.TipoId = t.Id";

            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(MapearPago(r));
            }
            return lista;
        }

        // --- Obtener por Id ---
        public Pago? ObtenerPorId(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
                SELECT 
                    p.Id, p.ContratoId, p.Fecha, p.Importe, p.MedioPago, p.Observaciones, p.Estado,
                    p.UsuarioCreacionPago, p.FechaCreacionPago,
                    p.UsuarioAnulacionPago, p.FechaAnulacionPago,
                    c.Id AS ContratoId,
                    i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
                    inm.Direccion AS InmuebleDireccion,
                    t.Descripcion AS InmuebleTipo
                FROM pago p
                INNER JOIN contrato c ON p.ContratoId = c.Id
                INNER JOIN inquilino i ON c.InquilinoId = i.Id
                INNER JOIN inmueble inm ON c.InmuebleId = inm.Id
                LEFT JOIN tipoinmueble t ON inm.TipoId = t.Id
                WHERE p.Id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (r.Read()) return MapearPago(r);
            return null;
        }

        // --- Alta con auditoría ---
        public int Alta(Pago p, string usuario)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
                INSERT INTO pago
                    (ContratoId, Fecha, Importe, MedioPago, Observaciones, Estado, UsuarioCreacionPago, FechaCreacionPago)
                VALUES
                    (@ContratoId, @Fecha, @Importe, @MedioPago, @Observaciones, @Estado, @Usuario, @FechaCreacion)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ContratoId", p.ContratoId);
            cmd.Parameters.AddWithValue("@Fecha", p.Fecha);
            cmd.Parameters.AddWithValue("@Importe", p.Importe);
            cmd.Parameters.AddWithValue("@MedioPago", (object?)p.MedioPago ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)p.Observaciones ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Estado", p.Estado ?? "Activo");
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            cmd.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // --- Modificación ---
        public int Modificacion(Pago p)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
                UPDATE pago SET 
                    Observaciones=@Observaciones,
                    Estado=@Estado
                WHERE Id=@Id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)p.Observaciones ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Estado", (object?)p.Estado ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", p.Id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // --- Anulación con auditoría ---
        public int Anular(int id, string usuario)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
                UPDATE pago SET 
                    Estado='Anulado', 
                    UsuarioAnulacionPago=@Usuario, 
                    FechaAnulacionPago=@Fecha
                WHERE Id=@Id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // --- Eliminar definitivo ---
        public int EliminarDefinitivo(int id)
        {
            int res = 0;
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                string sql = "DELETE FROM pago WHERE Id=@Id";
                using var cmd = new MySqlCommand(sql, conn, tx);
                cmd.Parameters.AddWithValue("@Id", id);
                res = cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            return res;
        }

        // --- Mapeo privado ---
        private Pago MapearPago(MySqlDataReader r)
        {
            return new Pago
            {
                Id = r.GetInt32("Id"),
                ContratoId = r.GetInt32("ContratoId"),
                Fecha = r.GetDateTime("Fecha"),
                Importe = r.GetDecimal("Importe"),
                MedioPago = r.IsDBNull(r.GetOrdinal("MedioPago")) ? null : r.GetString("MedioPago"),
                Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones"),
                Estado = r.IsDBNull(r.GetOrdinal("Estado")) ? "Activo" : r.GetString("Estado"),
                UsuarioCreacionPago = r.IsDBNull(r.GetOrdinal("UsuarioCreacionPago")) ? null : r.GetString("UsuarioCreacionPago"),
                FechaCreacionPago = r.IsDBNull(r.GetOrdinal("FechaCreacionPago")) ? DateTime.MinValue : r.GetDateTime("FechaCreacionPago"),
                UsuarioAnulacionPago = r.IsDBNull(r.GetOrdinal("UsuarioAnulacionPago")) ? null : r.GetString("UsuarioAnulacionPago"),
                FechaAnulacionPago = r.IsDBNull(r.GetOrdinal("FechaAnulacionPago")) ? (DateTime?)null : r.GetDateTime("FechaAnulacionPago"),

                ContratoInmuebleDireccion = r.IsDBNull(r.GetOrdinal("InmuebleDireccion")) ? null : r.GetString("InmuebleDireccion"),
                ContratoInmuebleTipo = r.IsDBNull(r.GetOrdinal("InmuebleTipo")) ? null : r.GetString("InmuebleTipo"),
                ContratoInquilinoNombre = r.IsDBNull(r.GetOrdinal("InquilinoNombre")) ? null : r.GetString("InquilinoNombre"),
                ContratoInquilinoApellido = r.IsDBNull(r.GetOrdinal("InquilinoApellido")) ? null : r.GetString("InquilinoApellido")
            };
        }
    }
}
