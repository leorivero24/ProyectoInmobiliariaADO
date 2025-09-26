using MySql.Data.MySqlClient;
using ProyectoInmobiliariaADO.Models;
using System.Collections.Generic;

namespace ProyectoInmobiliariaADO.Data
{
    public class RepositorioPropietario
    {
        private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

        // Obtener todos los propietarios
        public List<Propietario> ObtenerTodos()
        {
            var lista = new List<Propietario>();
            using var connection = new MySqlConnection(connectionString);
            string sql = "SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Direccion FROM propietario";
            using var command = new MySqlCommand(sql, connection);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var p = new Propietario
                {
                    Id = reader.GetInt32("Id"),
                    DNI = reader.GetString("DNI"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono"),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                    Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString("Direccion")
                };
                lista.Add(p);
            }
            return lista;
        }

        // Obtener propietario por Id
        public Propietario? ObtenerPorId(int id)
        {
            Propietario? p = null;
            using var connection = new MySqlConnection(connectionString);
            string sql = "SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Direccion FROM propietario WHERE Id=@id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                p = new Propietario
                {
                    Id = reader.GetInt32("Id"),
                    DNI = reader.GetString("DNI"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono"),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                    Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString("Direccion")
                };
            }
            return p;
        }

        // Obtener propietario por DNI
        public Propietario? ObtenerPorDNI(string dni)
        {
            Propietario? p = null;
            using var connection = new MySqlConnection(connectionString);
            string sql = "SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Direccion FROM propietario WHERE DNI=@DNI";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DNI", dni);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                p = new Propietario
                {
                    Id = reader.GetInt32("Id"),
                    DNI = reader.GetString("DNI"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono"),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                    Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString("Direccion")
                };
            }
            return p;
        }

        // Alta de propietario
        public int Alta(Propietario p)
        {
            int res = -1;
            using var connection = new MySqlConnection(connectionString);
            string sql = @"INSERT INTO propietario (DNI, Nombre, Apellido, Telefono, Email, Direccion) 
                           VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Email, @Direccion)";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DNI", p.DNI);
            command.Parameters.AddWithValue("@Nombre", p.Nombre);
            command.Parameters.AddWithValue("@Apellido", p.Apellido);
            command.Parameters.AddWithValue("@Telefono", (object?)p.Telefono ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)p.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@Direccion", (object?)p.Direccion ?? DBNull.Value);
            connection.Open();
            res = command.ExecuteNonQuery();
            return res;
        }

        // Modificación de propietario
        public int Modificacion(Propietario p)
        {
            int res = -1;
            using var connection = new MySqlConnection(connectionString);
            string sql = @"UPDATE propietario SET DNI=@DNI, Nombre=@Nombre, Apellido=@Apellido, 
                           Telefono=@Telefono, Email=@Email, Direccion=@Direccion WHERE Id=@Id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DNI", p.DNI);
            command.Parameters.AddWithValue("@Nombre", p.Nombre);
            command.Parameters.AddWithValue("@Apellido", p.Apellido);
            command.Parameters.AddWithValue("@Telefono", (object?)p.Telefono ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)p.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@Direccion", (object?)p.Direccion ?? DBNull.Value);
            command.Parameters.AddWithValue("@Id", p.Id);
            connection.Open();
            res = command.ExecuteNonQuery();
            return res;
        }

        // Baja de propietario
        public int Baja(int id)
        {
            int res = -1;
            using var connection = new MySqlConnection(connectionString);
            string sql = "DELETE FROM propietario WHERE Id=@id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            res = command.ExecuteNonQuery();
            return res;
        }
    }
}
