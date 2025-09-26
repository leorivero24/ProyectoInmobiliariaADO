using MySql.Data.MySqlClient;
using ProyectoInmobiliariaADO.Models;

namespace ProyectoInmobiliariaADO.Data
{
    public class RepositorioUsuario
    {
        private readonly string connectionString = "Server=127.0.0.1;Database=inmobiliariadb;User=root;Password=;";

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using var conn = new MySqlConnection(connectionString);

            string sql = @"SELECT Id, Nombre, Email, PasswordHash, Avatar, Rol, Activo, FechaCreacion, FechaUltimoLogin 
                           FROM usuario";

            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                lista.Add(new Usuario
                {
                    Id = r.GetInt32("Id"),
                    Nombre = r.GetString("Nombre"),
                    Email = r.GetString("Email"),
                    PasswordHash = r.GetString("PasswordHash"),
                    Avatar = r.IsDBNull(r.GetOrdinal("Avatar")) ? null : r.GetString("Avatar"),
                    Rol = (RolUsuario)r.GetInt32("Rol"),
                    Activo = r.GetBoolean("Activo"),
                    FechaCreacion = r.GetDateTime("FechaCreacion"),
                    FechaUltimoLogin = r.IsDBNull(r.GetOrdinal("FechaUltimoLogin")) ? null : r.GetDateTime("FechaUltimoLogin")
                });
            }

            return lista;
        }

        public Usuario? ObtenerPorId(int id)
        {
            Usuario? usuario = null;
            using var conn = new MySqlConnection(connectionString);

            string sql = @"SELECT Id, Nombre, Email, PasswordHash, Avatar, Rol, Activo, FechaCreacion, FechaUltimoLogin
                           FROM usuario
                           WHERE Id=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            using var r = cmd.ExecuteReader();

            if (r.Read())
            {
                usuario = new Usuario
                {
                    Id = r.GetInt32("Id"),
                    Nombre = r.GetString("Nombre"),
                    Email = r.GetString("Email"),
                    PasswordHash = r.GetString("PasswordHash"),
                    Avatar = r.IsDBNull(r.GetOrdinal("Avatar")) ? null : r.GetString("Avatar"),
                    Rol = (RolUsuario)r.GetInt32("Rol"),
                    Activo = r.GetBoolean("Activo"),
                    FechaCreacion = r.GetDateTime("FechaCreacion"),
                    FechaUltimoLogin = r.IsDBNull(r.GetOrdinal("FechaUltimoLogin")) ? null : r.GetDateTime("FechaUltimoLogin")
                };
            }

            return usuario;
        }

        public int Alta(Usuario u)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"INSERT INTO usuario 
                           (Nombre, Email, PasswordHash, Avatar, Rol, Activo, FechaCreacion) 
                           VALUES (@Nombre, @Email, @PasswordHash, @Avatar, @Rol, @Activo, @FechaCreacion)";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
            cmd.Parameters.AddWithValue("@Email", u.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", u.PasswordHash);
            cmd.Parameters.AddWithValue("@Avatar", (object?)u.Avatar ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Rol", (int)u.Rol);
            cmd.Parameters.AddWithValue("@Activo", u.Activo);
            cmd.Parameters.AddWithValue("@FechaCreacion", u.FechaCreacion);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Modificacion(Usuario u)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = @"UPDATE usuario 
                           SET Nombre=@Nombre, Email=@Email, PasswordHash=@PasswordHash, 
                               Avatar=@Avatar, Rol=@Rol, Activo=@Activo, FechaUltimoLogin=@FechaUltimoLogin
                           WHERE Id=@Id";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
            cmd.Parameters.AddWithValue("@Email", u.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", u.PasswordHash);
            cmd.Parameters.AddWithValue("@Avatar", (object?)u.Avatar ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Rol", (int)u.Rol);
            cmd.Parameters.AddWithValue("@Activo", u.Activo);
            cmd.Parameters.AddWithValue("@FechaUltimoLogin", (object?)u.FechaUltimoLogin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", u.Id);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Baja(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = "UPDATE usuario SET Activo=0 WHERE Id=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

             public int Eliminar(int id)
        {
            using var conn = new MySqlConnection(connectionString);
            string sql = "DELETE FROM usuario WHERE Id=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            Usuario? usuario = null;
            using var conn = new MySqlConnection(connectionString);

            string sql = @"SELECT Id, Nombre, Email, PasswordHash, Avatar, Rol, Activo, FechaCreacion, FechaUltimoLogin
                           FROM usuario
                           WHERE Email=@Email";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();
            using var r = cmd.ExecuteReader();

            if (r.Read())
            {
                usuario = new Usuario
                {
                    Id = r.GetInt32("Id"),
                    Nombre = r.GetString("Nombre"),
                    Email = r.GetString("Email"),
                    PasswordHash = r.GetString("PasswordHash"),
                    Avatar = r.IsDBNull(r.GetOrdinal("Avatar")) ? null : r.GetString("Avatar"),
                    Rol = (RolUsuario)r.GetInt32("Rol"),
                    Activo = r.GetBoolean("Activo"),
                    FechaCreacion = r.GetDateTime("FechaCreacion"),
                    FechaUltimoLogin = r.IsDBNull(r.GetOrdinal("FechaUltimoLogin")) ? null : r.GetDateTime("FechaUltimoLogin")
                };
            }

            return usuario;
        }

        
    }
}
