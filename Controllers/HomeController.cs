using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;
using Microsoft.Data.SqlClient;
using Dapper;  // 👈 necesario

namespace Eldertech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexSesionado()
        {
            using var conexion = BD.ObtenerConexion();

            var articulos = conexion.Query<Articulo>(
                "sp_GetUltimosArticulos",
                commandType: System.Data.CommandType.StoredProcedure
            );

            return View(articulos);
        }

        public IActionResult Articulos()
        {
            using var conexion = BD.ObtenerConexion();

            var articulos = conexion.Query<Articulo>(
                "sp_GetArticulos",
                commandType: System.Data.CommandType.StoredProcedure
            );

            return View(articulos);
        }

        public IActionResult IniciarSesion() => View();
        public IActionResult Registrarse() => View();
        public IActionResult RecuperarContraseña() => View();
        public IActionResult Mail() => View();

        public IActionResult Articulos2() => View();
    }
}
