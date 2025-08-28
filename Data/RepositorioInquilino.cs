using MySql.Data.MySqlClient;
using ProyectoInmobiliariaADO.Models;
using System;
using System.Collections.Generic;

namespace ProyectoInmobiliariaADO.Data
{
    public class RepositorioInquilino
    {
        private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

        public List<Inquilino> ObtenerTodos()
        {
            var lista = new List<Inquilino>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Direccion FROM inquilino";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Inquilino
                        {
                            Id = reader.GetInt32("Id"),
                            DNI = reader.GetString("DNI"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Direccion = reader.GetString("Direccion")
                        };
                        lista.Add(i);
                    }
                    connection.Close();
                }
            }
            return lista;
        }

        public Inquilino? ObtenerPorId(int id)
        {
            Inquilino? i = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Direccion FROM inquilino WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inquilino
                        {
                            Id = reader.GetInt32("Id"),
                            DNI = reader.GetString("DNI"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Direccion = reader.GetString("Direccion")
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        // ✅ Nuevo método para buscar por DNI
        public Inquilino? ObtenerPorDNI(string dni)
        {
            Inquilino? i = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Direccion FROM inquilino WHERE DNI=@DNI";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", dni);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inquilino
                        {
                            Id = reader.GetInt32("Id"),
                            DNI = reader.GetString("DNI"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Direccion = reader.GetString("Direccion")
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public int Alta(Inquilino i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO inquilino (DNI, Nombre, Apellido, Telefono, Email, Direccion) 
                               VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Email, @Direccion)";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", i.DNI);
                    command.Parameters.AddWithValue("@Nombre", i.Nombre);
                    command.Parameters.AddWithValue("@Apellido", i.Apellido);
                    command.Parameters.AddWithValue("@Telefono", i.Telefono);
                    command.Parameters.AddWithValue("@Email", i.Email);
                    command.Parameters.AddWithValue("@Direccion", i.Direccion);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inquilino i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE inquilino SET DNI=@DNI, Nombre=@Nombre, Apellido=@Apellido, 
                               Telefono=@Telefono, Email=@Email, Direccion=@Direccion WHERE Id=@Id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", i.DNI);
                    command.Parameters.AddWithValue("@Nombre", i.Nombre);
                    command.Parameters.AddWithValue("@Apellido", i.Apellido);
                    command.Parameters.AddWithValue("@Telefono", i.Telefono);
                    command.Parameters.AddWithValue("@Email", i.Email);
                    command.Parameters.AddWithValue("@Direccion", i.Direccion);
                    command.Parameters.AddWithValue("@Id", i.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM inquilino WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
    }
}
