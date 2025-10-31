using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;

namespace Eldertech.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult IndexSesionado()
        {
            var articulos = BD.GetUltimosArticulos() ?? new List<Articulo>();
            return View(articulos);
        }

        public IActionResult Articulos()
        {
            var articulos = BD.GetArticulos() ?? new List<Articulo>();
            return View(articulos);
        }

        public IActionResult IniciarSesion()
{
    return View("~/Views/Auth/IniciarSesion.cshtml");
}

        public IActionResult Registrarse()
{
    return View("~/Views/Auth/Registrarse.cshtml");
}

        public IActionResult RecuperarContraseña() => View();
        public IActionResult Mail() => View();
        public IActionResult Articulos2() => View();
    }
}
