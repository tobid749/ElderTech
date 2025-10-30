using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Eldertech.Controllers
{
    public class ArticulosController : Controller
    {
        [HttpGet]
        public IActionResult CrearArticulo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearArticulo(Articulo articulo)
        {
            using var conexion = BD.ObtenerConexion();

            articulo.Fecha = DateTime.Now;

            // ✅ Guardar artículo en BD
            var idArticulo = conexion.QuerySingle<int>(
                "sp_InsertArticulo",
                new
                {
                    articulo.Fecha,
                    articulo.Foto,
                    articulo.Titulo,
                    articulo.Subtitulo,
                    articulo.Texto,
                    articulo.Video
                },
                commandType: System.Data.CommandType.StoredProcedure
            );

            // ✅ Usuario por ahora fijo (hasta terminar login)
            conexion.Execute(
                "sp_InsertArticuloUsuario",
                new { IDUsuario = 1, IDArticulo = idArticulo },
                commandType: System.Data.CommandType.StoredProcedure
            );

            return RedirectToAction("Articulos", "Home");
        }
    }
}
