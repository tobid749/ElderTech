using System.Collections.Generic;
using System.Data;                 // ← necesario para CommandType
using Microsoft.Data.SqlClient;

using System.Linq;
using Dapper;
namespace Eldertech.Models
{
    public static class BD
    {
        private static string _connectionString =
            @"Server=localhost;Database=ElderTech;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion()
            => new SqlConnection(_connectionString);

        // --- Usuarios ---
        public static int RegistrarUsuario(Usuario user)
        {
            using var db = ObtenerConexion();
            return db.QuerySingle<int>("sp_RegistrarUsuario", new
            {
                user.NombreUsuario,
                user.Mail,
                user.Password,
                user.FechaNacimiento
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public static Usuario IniciarSesion(string nombreUsuario, string password)
        {
            using var db = ObtenerConexion();
            return db.QueryFirstOrDefault<Usuario>("sp_IniciarSesion", new
            {
                NombreUsuario = nombreUsuario,
                Password = password
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

 public static List<Aplicacion> ObtenerAplicaciones()
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Aplicacion>(
                    "sp_ObtenerAplicaciones",
                    commandType: CommandType.StoredProcedure   // ← ahora compila
                ).ToList();
            }
        }

        public static Aplicacion ObtenerAplicacionPorId(int id)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                return db.QueryFirstOrDefault<Aplicacion>(
                    "sp_ObtenerAplicacionPorId",
                    new { IDAplicacion = id },
                    commandType: CommandType.StoredProcedure   // ← ahora compila
                );
            }
        }

      

       

      

       

        
    }
}
