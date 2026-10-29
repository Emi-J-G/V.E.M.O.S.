namespace VEMOS.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using VEMOS.Models;
public class DB
{
    string _connectionString = @"Server=localhost;DataBase=VEMOSDB;Integrated Security=True;TrustServerCertificate=True;";
    
    //agarra un objeto usuario y lo inserta en la base de datos, incluyendo sus restricciones.
    public void registrarUsuario(Usuario usuario) {
        string query = "exec insertUser @username, @nombre, @apellido, @email, @contrasena, @tipo";
        string rquery = @"insertRestricciones @idUsuario, @nombre";
        using (SqlConnection connection = new SqlConnection(_connectionString)) { 
            connection.Open(); //mantiene la bd abierta mientras se ejecuta el using, es mas eficiente bajo varios inserts.
            //inserta user
            int nuevoId = connection.ExecuteScalar<int>(query, username = usuario.username, nombre = usuario.nombre, apellido = usuario.apellido, email = usaurio.email, contrasena = usuario.contrasena, tipo = usuario.tipo);
            usuario.id = nuevoId;
            //inserta restricciones
            if (usuario.restricciones != null && usuario.restricciones.Any()) {
                foreach (var restriccion in usuario.restricciones) {
                    if (string.IsNullOrWhiteSpace(restriccion.nombre))
                        continue;
                    connection.Execute(rquery, new { idUsuario = usuario.id, nombre = restriccion.nombre.Trim() });
                }
            }
        }
    }

    //devuelve un usuario si existe en la base de datos, si no devuelve null.
    public Usuario validarUsuario(string email, string contraseña) {
        string query = "exec validateUserByEmNPass @pemail, @pass";
        string rquery = @"exec getRestriccionesByidUser @idUser";
        using (SqlConnection connection = new SqlConnection(_connectionString)) {
            connection.Open();//mantiene la bd abierta mientras se ejecuta el using, es mas eficiente bajo varios inserts.
            //busca el usuario
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { pemail = email, pass = contraseña });
            if (usuario != null) {
                //busca las restricciones del usuario
                List<Restriccion> restricciones = connection.Query<Restriccion>(rquery, new { idUser = usuario.id }).ToList();
                usuario.restricciones = restricciones;
            }
        }
        return usuario;
    }

    //trae un usuario si su user existe
    public Usuario nomUsuarioExiste(string username) {
        string query = "exec getUsuarioByUsername @pusername";
        using (SqlConnection connection = new SqlConnection(_connectionString)) {
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { pusername = username});
        }
        return usuario;
    }
    public Usuario emUsuarioExiste(string email) {
        string query = "exec getUsuarioByEmail @pemail";
        using (SqlConnection connection = new SqlConnection(_connectionString)) {
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { pemail = email});
        }
        return usuario;
    }
}
