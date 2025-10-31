using Microsoft.Data.SqlClient;
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

        // --- Artículos ---
        public static int InsertArticulo(Articulo a)
        {
            using var db = ObtenerConexion();
            return db.QuerySingle<int>("sp_InsertArticulo", new
            {
                a.Fecha,
                a.Foto,
                a.FotoContentType,
                a.Titulo,
                a.Subtitulo,
                a.Texto,
                a.Video,
                a.AutorNombre
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public static void UpdateArticulo(Articulo a)
        {
            using var db = ObtenerConexion();
            db.Execute("sp_UpdateArticulo", new
            {
                a.IDArticulo,
                a.Fecha,
                a.Foto,
                a.FotoContentType,
                a.Titulo,
                a.Subtitulo,
                a.Texto,
                a.Video,
                a.AutorNombre
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public static void DeleteArticulo(int id)
        {
            using var db = ObtenerConexion();
            db.Execute("sp_DeleteArticulo", new { IDArticulo = id },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public static Articulo? GetArticuloById(int id)
        {
            using var db = ObtenerConexion();
            return db.QueryFirstOrDefault<Articulo>("sp_GetArticuloById",
                new { IDArticulo = id },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public static List<Articulo> GetArticulos()
        {
            using var db = ObtenerConexion();
            return db.Query<Articulo>("sp_GetArticulos",
                commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        public static List<Articulo> GetUltimosArticulos()
        {
            using var db = ObtenerConexion();
            return db.Query<Articulo>("sp_GetUltimosArticulos",
                commandType: System.Data.CommandType.StoredProcedure).ToList();
        }
    }
}
