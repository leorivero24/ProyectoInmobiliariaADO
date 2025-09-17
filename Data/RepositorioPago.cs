using MySql.Data.MySqlClient;
using ProyectoInmobiliariaADO.Models;
using System.Collections.Generic;

namespace ProyectoInmobiliariaADO.Data
{
    public class RepositorioPago
    {
        private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

        // Obtener todos los pagos de un contrato
        public List<Pago> ObtenerPorContrato(int contratoId)
        {
            var lista = new List<Pago>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, ContratoId, Fecha, Importe, MedioPago, Observaciones
                               FROM pago
                               WHERE ContratoId = @contratoId
                               ORDER BY Fecha ASC";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@contratoId", contratoId);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Pago
                        {
                            Id = reader.GetInt32("Id"),
                            ContratoId = reader.GetInt32("ContratoId"),
                            Fecha = reader.GetDateTime("Fecha"),
                            Monto = reader.GetDecimal("Importe"),
                            MedioPago = reader["MedioPago"] == DBNull.Value ? null : reader.GetString("MedioPago"),
                            Observaciones = reader["Observaciones"] == DBNull.Value ? null : reader.GetString("Observaciones")
                        });
                    }
                    connection.Close();
                }
            }
            return lista;
        }

        // Alta de un pago
        public int Alta(Pago p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO pago (ContratoId, Fecha, Importe, MedioPago, Observaciones)
                               VALUES (@ContratoId, @Fecha, @Monto, @MedioPago, @Observaciones)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ContratoId", p.ContratoId);
                    command.Parameters.AddWithValue("@Fecha", p.Fecha);
                    command.Parameters.AddWithValue("@Monto", p.Monto);
                    command.Parameters.AddWithValue("@MedioPago", (object?)p.MedioPago ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Observaciones", (object?)p.Observaciones ?? DBNull.Value);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        // Modificación de un pago
        public int Modificacion(Pago p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE pago
                               SET ContratoId=@ContratoId, Fecha=@Fecha, Importe=@Monto, MedioPago=@MedioPago, Observaciones=@Observaciones
                               WHERE Id=@Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ContratoId", p.ContratoId);
                    command.Parameters.AddWithValue("@Fecha", p.Fecha);
                    command.Parameters.AddWithValue("@Monto", p.Monto);
                    command.Parameters.AddWithValue("@MedioPago", (object?)p.MedioPago ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Observaciones", (object?)p.Observaciones ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Id", p.Id);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        // Baja de un pago
        public int Baja(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM pago WHERE Id=@Id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
    }
}
