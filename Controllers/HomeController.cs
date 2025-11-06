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
        public IActionResult Articulos()
{
    return View();
}
public IActionResult Aplicaciones()
        {
            var apps = BD.ObtenerAplicaciones();
            return View(apps);
        }

        // ✅ Detalle por id
     public IActionResult Aplicacion(int id, int? index)
{
    var app = BD.ObtenerAplicacionPorId(id);
    if (app == null) return RedirectToAction("Aplicaciones");

    var preguntas = BD.ObtenerPreguntasPorAplicacion(id);

    if (preguntas.Count == 0)
    {
        ViewBag.PreguntaActual = null;
        return View(app);
    }

    int pos = index ?? 0;
    if (pos >= preguntas.Count) pos = 0;

    ViewBag.PreguntaActual = preguntas[pos];
    ViewBag.NextIndex = pos + 1;
    ViewBag.TotalPreguntas = preguntas.Count;

    return View(app);
}

public IActionResult Pregunta(int id)
{
    var app = BD.ObtenerAplicacionPorId(id);
    var preguntas = BD.ObtenerPreguntasPorAplicacion(id);

    if (app == null || preguntas.Count == 0)
        return RedirectToAction("Aplicaciones");

    Random rnd = new Random();
    int index = rnd.Next(preguntas.Count);

    var pregunta = preguntas[index];

    // Armamos lista de opciones mezcladas
    var opciones = new List<(string text, int num)>
    {
        (pregunta.Opcion1, 1),
        (pregunta.Opcion2, 2),
        (pregunta.Opcion3, 3),
        (pregunta.Opcion4, 4)
    };

    opciones = opciones.OrderBy(x => rnd.Next()).ToList();

    ViewBag.PreguntaActual = pregunta;
    ViewBag.OpcionesMezcladas = opciones;

    return View("Aplicacion", app);
}



[HttpPost]
[HttpPost]
public IActionResult Siguiente(int id, int next)
{
    return RedirectToAction("Aplicacion", new { id = id, index = next });
}



    }
}
