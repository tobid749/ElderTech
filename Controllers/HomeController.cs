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
        public IActionResult Foro() => View();
        public IActionResult ForoSesionado(int offset = 0)
{
    var mensajes = BD.ObtenerMensajes(offset);
    ViewBag.Offset = offset;
    ViewBag.MostrarMas = mensajes.Count == 6; // Si hay 6, mostrar botón
    return View(mensajes);
}

[HttpPost]
public IActionResult PublicarMensaje(string mensaje)
{
    if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioNombre")))
        return RedirectToAction("IniciarSesion", "Home");

    string usuario = HttpContext.Session.GetString("UsuarioNombre");
    BD.AgregarMensaje(usuario, mensaje, "/Imagenes/AvatarDefault.png");

    return RedirectToAction("ForoSesionado");
}
        public IActionResult Mail() => View();
        public IActionResult Articulos2() => View();
        public IActionResult Articulos3() => View();
        public IActionResult Articulos4() => View();
        public IActionResult Articulos() => View();
        public IActionResult NecesitoAyuda() => View();
        public IActionResult Contacto() => View();

        // ✅ Página principal de Aplicaciones
        public IActionResult Aplicaciones()
        {
            var apps = BD.ObtenerAplicaciones();
            return View(apps);
        }

        // ✅ Muestra la aplicación seleccionada con sus preguntas
        public IActionResult Aplicacion(int id, int? index)
        {
            var app = BD.ObtenerAplicacionPorId(id);
            if (app == null) return RedirectToAction("Aplicaciones");

            var preguntas = BD.ObtenerPreguntasPorAplicacion(id);

            if (preguntas == null || preguntas.Count == 0)
            {
                ViewBag.PreguntaActual = null;
                return View(app);
            }

            // índice actual (posición de la pregunta)
            int pos = index ?? 0;
            if (pos >= preguntas.Count) pos = 0;

            var preguntaActual = preguntas[pos];

            // 🔀 Mezclamos las opciones
            Random rnd = new();
            var opciones = new List<(string texto, int num)>
            {
                (preguntaActual.Opcion1, 1),
                (preguntaActual.Opcion2, 2),
                (preguntaActual.Opcion3, 3),
                (preguntaActual.Opcion4, 4)
            };
            opciones = opciones.OrderBy(x => rnd.Next()).ToList();

            ViewBag.PreguntaActual = preguntaActual;
            ViewBag.OpcionesMezcladas = opciones;
            ViewBag.NextIndex = pos + 1;
            ViewBag.TotalPreguntas = preguntas.Count;

            return View(app);
        }

        // ✅ Botón siguiente
        [HttpPost]
        public IActionResult Siguiente(int id, int next)
        {
            return RedirectToAction("Aplicacion", new { id = id, index = next });
        }
    }
}
