
using MySql.Data.MySqlClient;
using ProyectoInmobiliariaADO.Models;
using System.Collections.Generic;

namespace ProyectoInmobiliariaADO.Data
{
    public class RepositorioInmueble
    {
        private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

        public List<Inmueble> ObtenerTodos()
        {
            var lista = new List<Inmueble>();
            using var conn = new MySqlConnection(connectionString);

            string sql = @"
SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Precio,
       i.PropietarioId, i.Estado, i.Observaciones,
       i.TipoId,
       p.Nombre AS PropietarioNombre, p.Apellido AS PropietarioApellido,
       t.Id AS TipoInmuebleId, t.Descripcion AS TipoDescripcion
FROM inmueble i
INNER JOIN propietario p ON i.PropietarioId = p.Id
INNER JOIN tipoinmueble t ON i.TipoId = t.Id";

            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Inmueble
                {
                    Id = r.GetInt32("Id"),
                    Direccion = r.GetString("Direccion"),
                    TipoId = r.GetInt32("TipoId"),
                    Tipo = new TipoInmueble
                    {
                        Id = r.GetInt32("TipoInmuebleId"),
                        Descripcion = r.GetString("TipoDescripcion")
                    },
                    Ambientes = r.IsDBNull(r.GetOrdinal("Ambientes")) ? 0 : r.GetInt32("Ambientes"),
                    Superficie = r.IsDBNull(r.GetOrdinal("Superficie")) ? 0 : r.GetDecimal("Superficie"),
                    Precio = r.IsDBNull(r.GetOrdinal("Precio")) ? 0 : r.GetDecimal("Precio"),
                    PropietarioId = r.GetInt32("PropietarioId"),
                    Estado = r.GetString("Estado"),
                    Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones"),
                    PropietarioNombre = r.IsDBNull(r.GetOrdinal("PropietarioNombre")) ? null : r.GetString("PropietarioNombre"),
                    PropietarioApellido = r.IsDBNull(r.GetOrdinal("PropietarioApellido")) ? null : r.GetString("PropietarioApellido")
                });
            }
            return lista;
        }

        public Inmueble? ObtenerPorId(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Precio,
       i.PropietarioId, i.Estado, i.Observaciones,
       i.TipoId,
       t.Id AS TipoInmuebleId, t.Descripcion AS TipoDescripcion
FROM inmueble i
INNER JOIN tipoinmueble t ON i.TipoId = t.Id
WHERE i.Id = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                return new Inmueble
                {
                    Id = r.GetInt32("Id"),
                    Direccion = r.GetString("Direccion"),
                    TipoId = r.GetInt32("TipoId"),
                    Tipo = new TipoInmueble
                    {
                        Id = r.GetInt32("TipoInmuebleId"),
                        Descripcion = r.GetString("TipoDescripcion")
                    },
                    Ambientes = r.IsDBNull(r.GetOrdinal("Ambientes")) ? 0 : r.GetInt32("Ambientes"),
                    Superficie = r.IsDBNull(r.GetOrdinal("Superficie")) ? 0 : r.GetDecimal("Superficie"),
                    Precio = r.IsDBNull(r.GetOrdinal("Precio")) ? 0 : r.GetDecimal("Precio"),
                    PropietarioId = r.GetInt32("PropietarioId"),
                    Estado = r.GetString("Estado"),
                    Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones")
                };
            }
            return null;
        }

        public int Alta(Inmueble m)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
INSERT INTO inmueble (Direccion, TipoId, Ambientes, Superficie, Precio, PropietarioId, Estado, Observaciones)
VALUES (@Direccion, @TipoId, @Ambientes, @Superficie, @Precio, @PropietarioId, @Estado, @Observaciones)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Direccion", m.Direccion);
            cmd.Parameters.AddWithValue("@TipoId", m.TipoId);
            cmd.Parameters.AddWithValue("@Ambientes", m.Ambientes);
            cmd.Parameters.AddWithValue("@Superficie", m.Superficie);
            cmd.Parameters.AddWithValue("@Precio", m.Precio);
            cmd.Parameters.AddWithValue("@PropietarioId", m.PropietarioId);
            cmd.Parameters.AddWithValue("@Estado", m.Estado);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)m.Observaciones ?? DBNull.Value);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Modificacion(Inmueble m)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"
UPDATE inmueble SET 
    Direccion=@Direccion, TipoId=@TipoId, Ambientes=@Ambientes, Superficie=@Superficie,
    Precio=@Precio, PropietarioId=@PropietarioId, Estado=@Estado, Observaciones=@Observaciones
WHERE Id=@Id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Direccion", m.Direccion);
            cmd.Parameters.AddWithValue("@TipoId", m.TipoId);
            cmd.Parameters.AddWithValue("@Ambientes", m.Ambientes);
            cmd.Parameters.AddWithValue("@Superficie", m.Superficie);
            cmd.Parameters.AddWithValue("@Precio", m.Precio);
            cmd.Parameters.AddWithValue("@PropietarioId", m.PropietarioId);
            cmd.Parameters.AddWithValue("@Estado", m.Estado);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)m.Observaciones ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", m.Id);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Baja(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = "DELETE FROM inmueble WHERE Id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public List<Inmueble> ObtenerDisponiblesEntreFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<Inmueble>();
            using var conn = new MySqlConnection(connectionString);

            string sql = @"
SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Precio,
       i.PropietarioId, i.Estado, i.Observaciones,
       i.TipoId,
       p.Nombre AS PropietarioNombre, p.Apellido AS PropietarioApellido,
       t.Id AS TipoInmuebleId, t.Descripcion AS TipoDescripcion
FROM inmueble i
INNER JOIN propietario p ON i.PropietarioId = p.Id
INNER JOIN tipoinmueble t ON i.TipoId = t.Id
WHERE i.Id NOT IN (
    SELECT c.InmuebleId
    FROM contrato c
    WHERE NOT (c.FechaFin < @fechaInicio OR c.FechaInicio > @fechaFin)
)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Inmueble
                {
                    Id = r.GetInt32("Id"),
                    Direccion = r.GetString("Direccion"),
                    TipoId = r.GetInt32("TipoId"),
                    Tipo = new TipoInmueble
                    {
                        Id = r.GetInt32("TipoInmuebleId"),
                        Descripcion = r.GetString("TipoDescripcion")
                    },
                    Ambientes = r.IsDBNull(r.GetOrdinal("Ambientes")) ? 0 : r.GetInt32("Ambientes"),
                    Superficie = r.IsDBNull(r.GetOrdinal("Superficie")) ? 0 : r.GetDecimal("Superficie"),
                    Precio = r.IsDBNull(r.GetOrdinal("Precio")) ? 0 : r.GetDecimal("Precio"),
                    PropietarioId = r.GetInt32("PropietarioId"),
                    Estado = r.GetString("Estado"),
                    Observaciones = r.IsDBNull(r.GetOrdinal("Observaciones")) ? null : r.GetString("Observaciones"),
                    PropietarioNombre = r.IsDBNull(r.GetOrdinal("PropietarioNombre")) ? null : r.GetString("PropietarioNombre"),
                    PropietarioApellido = r.IsDBNull(r.GetOrdinal("PropietarioApellido")) ? null : r.GetString("PropietarioApellido")
                });
            }

            return lista;
        }



    }
}
