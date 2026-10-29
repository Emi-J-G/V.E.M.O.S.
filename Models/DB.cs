namespace VEMOS.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using VEMOS.Models;
public class DB
{
    string _connectionString = @"Server=localhost;DataBase=VEMOSDB;Integrated Security=True;TrustServerCertificate=True;";
    
    //agarra un objeto usuario y lo inserta en la base de datos, incluyendo sus restricciones.
    public void registrarUsuario(Usuario usuario) {
        using (SqlConnection connection = new SqlConnection(_connectionString)) { 
            connection.Open(); //mantiene la bd abierta mientras se ejecuta el using, es mas eficiente bajo varios inserts.

            //inserta user
            string query = "exec insertUser @username, @nombre, @apellido, @email, @contrasena, @tipo";
            int nuevoId = connection.ExecuteScalar<int>(query, usuario);
            usuario.id = nuevoId;

            //inserta restricciones
            if (usuario.restricciones != null && usuario.restricciones.Any()) {
                foreach (var restriccion in usuario.restricciones) {
                    if (string.IsNullOrWhiteSpace(restriccion.nombre))
                        continue;

                    string rquery = @"INSERT INTO RestriccionesUsuarios (idRestriccion, idUsuario)
                                      SELECT id, @idUsuario
                                      FROM Restricciones
                                      WHERE nombre = @nombre";
                    connection.Execute(rquery, new { idUsuario = usuario.id, nombre = restriccion.nombre.Trim() });
                }
            }
        }
    }

    //devuelve un usuario si existe en la base de datos, sino devuelve null.
    public Usuario validarUsuario(string email, string contraseña) {
        string query = "SELECT * FROM Usuarios WHERE email = @pemail AND contrasena = @pcontrasena";
        string rquery = @"SELECT r.id, r.nombre, r.descripcion FROM RestriccionesUsuarios ru
                                JOIN Restricciones r ON ru.idRestriccion = r.id WHERE ru.idUsuario = @pidUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString)) {
            connection.Open();//mantiene la bd abierta mientras se ejecuta el using, es mas eficiente bajo varios inserts.
            //busca el usuario
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { pemail = email, pcontrasena = contraseña });
            if (usuario == null)
                return null;
            //busca las restricciones del usuario
            List<Restriccion> restricciones = connection.Query<Restriccion>(rquery, new { pidUsuario = usuario.id }).ToList();
            usuario.restricciones = restricciones;

            return usuario;
        }
    }

    //trae un usuario si su user existe
    public Usuario nomUsuarioExiste(string username) {
        string query = "SELECT * FROM Usuarios WHERE username = @pusername";
        using (SqlConnection connection = new SqlConnection(_connectionString)) {
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { pusername = username});
            return usuario;
        }
    }
    public Usuario emUsuarioExiste(string email) {
        string query = "SELECT * FROM Usuarios WHERE email = @pemail";
        using (SqlConnection connection = new SqlConnection(_connectionString)) {
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { pemail = email});
            return usuario;
        }
    }
}
