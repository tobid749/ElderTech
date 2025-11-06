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

        public static List<PreguntaAplicacion> ObtenerPreguntasPorAplicacion(int idApp)
{
    using (SqlConnection db = new SqlConnection(_connectionString))
    {
        return db.Query<PreguntaAplicacion>(
            "sp_ObtenerPreguntasPorAplicacion",
            new { idApp },
            commandType: CommandType.StoredProcedure
        ).ToList();
    }
}
public static List<Articulo> ObtenerArticulosPorAplicacion(int idApp)
{
    return new List<Articulo>(); 
}
public static List<PreguntaAplicacion> ObtenerPreguntasPorAplicacion(int idAplicacion)
{
    List<PreguntaAplicacion> lista = new();

    using (SqlConnection con = new SqlConnection(_connectionString))
    {
        string sql = "SELECT * FROM PreguntasAplicacion WHERE IDAplicacion = @id ORDER BY NEWID()";
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@id", idAplicacion);

        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            lista.Add(new PreguntaAplicacion
            {
                IDPregunta = dr.GetInt32(0),
                IDAplicacion = dr.GetInt32(1),
                Enunciado = dr.GetString(2),
                Opcion1 = dr.GetString(3),
                Opcion2 = dr.GetString(4),
                Opcion3 = dr.GetString(5),
                Opcion4 = dr.GetString(6),
                Correcta = dr.GetInt32(7)
            });
        }

        dr.Close();
    }

    return lista;
}


      

       

      

       

        
    }
}
