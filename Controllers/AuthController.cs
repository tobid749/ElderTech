using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;

namespace Eldertech.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Registrarse()
        {
            return View("~/Views/Auth/Registrarse.cshtml");
        }

        [HttpPost]
        public IActionResult Registrarse(Usuario u)
        {
            BD.RegistrarUsuario(u);
            HttpContext.Session.SetString("UsuarioNombre", u.NombreUsuario);
            return RedirectToAction("IndexSesionado", "Home");
        }

        [HttpPost]
        public IActionResult IniciarSesion(string NombreUsuario, string Password)
        {
            var user = BD.IniciarSesion(NombreUsuario, Password);

            if (user != null)
            {
                HttpContext.Session.SetString("UsuarioNombre", user.NombreUsuario);
                return RedirectToAction("IndexSesionado", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View("~/Views/Auth/IniciarSesion.cshtml");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
