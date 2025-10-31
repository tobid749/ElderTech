using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;

namespace Eldertech.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult IniciarSesion(string Usuario, string Password)
        {
            var user = BD.IniciarSesion(Usuario, Password);

            if (user != null)
            {
                // Sesión para autor
                HttpContext.Session.SetString("UsuarioNombre", user.NombreUsuario);
                return RedirectToAction("IndexSesionado", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View("../Home/IniciarSesion");
        }

        [HttpPost]
        public IActionResult Registrarse(Usuario u)
        {
            BD.RegistrarUsuario(u);
            // Autologin opcional:
            HttpContext.Session.SetString("UsuarioNombre", u.NombreUsuario);
            return RedirectToAction("IndexSesionado", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}
