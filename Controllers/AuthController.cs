using Microsoft.Data.SqlClient;
using Dapper;

namespace Eldertech.Models
{
    public static class BD
    {
        private static string _connectionString = 
            @"server=localhost;DataBase=ElderTechSQL;Integrated Security=true;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_connectionString);
        }

        public static int RegistrarUsuario(Usuario u)
        {
            using var db = ObtenerConexion();
            return db.QuerySingle<int>("sp_RegistrarUsuario", u,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public static Usuario IniciarSesion(string usuario, string password)
        {
            using var db = ObtenerConexion();
            return db.QueryFirstOrDefault<Usuario>(
                "sp_IniciarSesion",
                new { Usuario = usuario, Password = password },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }
}
