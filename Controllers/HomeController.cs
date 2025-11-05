using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;

namespace Eldertech.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

       public IActionResult IndexSesionado()
        {
            return View();
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
        public IActionResult Articulos3() => View();
        public IActionResult Articulos4() => View();
        public IActionResult Aplicaciones() => View();
        public IActionResult Articulos()
{
    return View();
}

    }
}
