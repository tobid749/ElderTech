using System.Collections.Generic;
using System.Data;                 
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

        // --- USUARIOS ---
        public static int RegistrarUsuario(Usuario user)
        {
            using var db = ObtenerConexion();
            return db.QuerySingle<int>("sp_RegistrarUsuario", new
            {
                user.NombreUsuario,
                user.Mail,
                user.Password,
                user.FechaNacimiento
            }, commandType: CommandType.StoredProcedure);
        }

        public static Usuario IniciarSesion(string nombreUsuario, string password)
        {
            using var db = ObtenerConexion();
            return db.QueryFirstOrDefault<Usuario>("sp_IniciarSesion", new
            {
                NombreUsuario = nombreUsuario,
                Password = password
            }, commandType: CommandType.StoredProcedure);
        }

        // --- APLICACIONES ---
        public static List<Aplicacion> ObtenerAplicaciones()
        {
            using var db = new SqlConnection(_connectionString);
            return db.Query<Aplicacion>(
                "sp_ObtenerAplicaciones",
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public static Aplicacion ObtenerAplicacionPorId(int id)
        {
            using var db = new SqlConnection(_connectionString);
            return db.QueryFirstOrDefault<Aplicacion>(
                "sp_ObtenerAplicacionPorId",
                new { IDAplicacion = id },
                commandType: CommandType.StoredProcedure
            );
        }

        // --- PREGUNTAS ---
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

        // --- ARTÍCULOS ---
        public static List<Articulo> ObtenerArticulosPorAplicacion(int idApp)
        {
            // Implementación futura
            return new List<Articulo>();
        }
        // --- FORO ---
public static List<ForoMensaje> ObtenerMensajes(int offset = 0)
{
    using var db = ObtenerConexion();
    return db.Query<ForoMensaje>("sp_ObtenerMensajes", new { Offset = offset },
        commandType: CommandType.StoredProcedure).ToList();
}

public static void AgregarMensaje(string nombreUsuario, string mensaje, string avatar)
{
    using var db = ObtenerConexion();
    db.Execute("sp_AgregarMensaje", new
    {
        NombreUsuario = nombreUsuario,
        Mensaje = mensaje,
        Avatar = avatar ?? "/Imagenes/default-avatar.png"
    }, commandType: CommandType.StoredProcedure);
}
 // ---------------- CAMINO --------------------

        public static List<CaminoNivel> ObtenerNiveles(int idUsuario)
        {
            using var db = ObtenerConexion();
            return db.Query<CaminoNivel>(
                "sp_Camino_GetNiveles",
                new { IDUsuario = idUsuario },
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public static List<CaminoPregunta> ObtenerPreguntasCamino(int nivel)
        {
            using var db = ObtenerConexion();
            return db.Query<CaminoPregunta>(
                "sp_Camino_GetPreguntas",
                new { IDNivel = nivel },
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public static void GuardarResultadoCamino(int idUsuario, int nivel, int estrellas)
{
    using var db = ObtenerConexion();
    db.Execute("sp_Camino_GuardarResultado",
        new { IDUsuario = idUsuario, Nivel = nivel, Estrellas = estrellas },
        commandType: CommandType.StoredProcedure);
}


        public static int GetUsuarioId(string nombreUsuario)
        {
            using var db = ObtenerConexion();
            return db.QuerySingle<int>(
                "SELECT IDUsuario FROM Usuarios WHERE NombreUsuario = @n",
                new { n = nombreUsuario }
            );
        }

        // ---------------- ARTÍCULOS ----------------

public static List<Articulo> ObtenerArticulos()
{
    using var db = ObtenerConexion();
    string sql = "SELECT * FROM Articulo ORDER BY Fecha DESC";
    return db.Query<Articulo>(sql).ToList();
}

public static Articulo ObtenerArticuloPorId(int id)
{
    using var db = ObtenerConexion();
    string sql = "SELECT * FROM Articulo WHERE IDArticulo = @id";
    return db.QueryFirstOrDefault<Articulo>(sql, new { id });
}
public static List<Articulo> TraerArticulos()
{
    List<Articulo> lista = new List<Articulo>();

    using (SqlConnection cn = new SqlConnection(_connectionString))
    {
        cn.Open();

        SqlCommand cmd = new SqlCommand("TraerArticulos", cn);
        cmd.CommandType = CommandType.StoredProcedure;

        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            Articulo art = new Articulo
            {
                IDArticulo = dr.GetInt32(dr.GetOrdinal("IDArticulo")),
                Fecha = dr.GetDateTime(dr.GetOrdinal("Fecha")),
                Foto = dr["Foto"].ToString(),
                Titulo = dr["Titulo"].ToString(),
                Subtitulo = dr["Subtitulo"] as string,
                Texto = dr["Texto"] as string,
                Video = dr["Video"] as string,
                Autor = dr["Autor"].ToString()
            };

            lista.Add(art);
        }
    }

    return lista;
}


    }
}
    
    
