using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;

namespace Eldertech.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult IndexSesionado() => View();

        public IActionResult IniciarSesion()
        {
            return View("~/Views/Auth/IniciarSesion.cshtml");
        }

        public IActionResult Registrarse()
        {
            return View("~/Views/Auth/Registrarse.cshtml");
        }

        public IActionResult RecuperarContraseña() => View();

        // ------------------ FORO ------------------
        public IActionResult Foro()
        {
            var mensajes = BD.ObtenerMensajes(0);
            ViewBag.Offset = 0;
            ViewBag.MostrarMas = mensajes.Count == 6;
            return View(mensajes);
        }

        public IActionResult ForoSesionado(int offset = 0)
        {
            var mensajes = BD.ObtenerMensajes(offset);
            ViewBag.Offset = offset;
            ViewBag.MostrarMas = mensajes.Count == 6;
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

        // ------------------ ARTÍCULOS / PÁGINAS ------------------
        public IActionResult Mail() => View();
        public IActionResult Articulos2() => View();
        public IActionResult Articulos3() => View();
        public IActionResult Articulos4() => View();
        public IActionResult Articulos() => View();
        public IActionResult NecesitoAyuda() => View();
        public IActionResult Contacto() => View();

        // ------------------ APLICACIONES ------------------
        public IActionResult Aplicaciones()
        {
            var apps = BD.ObtenerAplicaciones();
            return View(apps);
        }

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

            int pos = index ?? 0;
            if (pos >= preguntas.Count) pos = 0;

            var preguntaActual = preguntas[pos];

            Random rnd = new();
            var opciones = new List<(string texto, int num)>
            {
                (preguntaActual.Opcion1, 1),
                (preguntaActual.Opcion2, 2),
                (preguntaActual.Opcion3, 3),
                (preguntaActual.Opcion4, 4)
            }
            .OrderBy(x => rnd.Next())
            .ToList();

            ViewBag.PreguntaActual = preguntaActual;
            ViewBag.OpcionesMezcladas = opciones;
            ViewBag.NextIndex = pos + 1;
            ViewBag.TotalPreguntas = preguntas.Count;

            return View(app);
        }

        [HttpPost]
        public IActionResult Siguiente(int id, int next)
        {
            return RedirectToAction("Aplicacion", new { id = id, index = next });
        }

        // ------------------ CAMINO ------------------
    public IActionResult Camino()
        {
            string usuario = HttpContext.Session.GetString("UsuarioNombre");
            if (usuario == null)
                return RedirectToAction("IniciarSesion");

            int idUsuario = BD.GetUsuarioId(usuario);

            var niveles = BD.ObtenerNiveles(idUsuario);

            return View(niveles);
        }

        public IActionResult Nivel(int id)
        {
            var preguntas = BD.ObtenerPreguntasCamino(id);
            ViewBag.Nivel = id;
            return View(preguntas);
        }

        [HttpPost]
        public IActionResult GuardarResultado(int nivel, int estrellas)
        {
            int idUsuario = BD.GetUsuarioId(HttpContext.Session.GetString("UsuarioNombre"));
            BD.GuardarResultadoCamino(idUsuario, nivel, estrellas);

            return RedirectToAction("Camino");
        }
    }
}
