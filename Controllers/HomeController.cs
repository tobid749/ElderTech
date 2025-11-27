using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;

namespace Eldertech.Controllers
{
    public class HomeController : Controller
    {
        private bool UsuarioLogueado()
{
    return !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioNombre"));
}

private IActionResult RedirigirSegunSesion(string vistaSinSesion, string vistaConSesion)
{
    return UsuarioLogueado() ? View(vistaConSesion) : View(vistaSinSesion);
}
public IActionResult Logout()
{
    HttpContext.Session.Clear();
    return RedirectToAction("Index");
}

        public IActionResult Index() => View();
        public IActionResult IndexSesionado()
{
    if (!UsuarioLogueado())
        return RedirectToAction("IniciarSesion");
CargarDatosUsuario();
    return View();
}



        public IActionResult IniciarSesion()
        {
            if (UsuarioLogueado()) return RedirectToAction("IndexSesionado");

            return View("~/Views/Auth/IniciarSesion.cshtml");
        }

        public IActionResult Registrarse()
        {
            if (UsuarioLogueado()) return RedirectToAction("IndexSesionado");

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
    if (!UsuarioLogueado())
        return RedirectToAction("IniciarSesion");

    var mensajes = BD.ObtenerMensajes(offset);
    ViewBag.Offset = offset;
    ViewBag.MostrarMas = mensajes.Count == 6;
CargarDatosUsuario();

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
        public IActionResult NecesitoAyuda() => View();
        public IActionResult Contacto() => View();

        // ------------------ APLICACIONES ------------------
        public IActionResult Aplicaciones()
{
    if (!UsuarioLogueado()) return RedirectToAction("IniciarSesion");

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
    if (!UsuarioLogueado())
        return RedirectToAction("IniciarSesion");

    string usuario = HttpContext.Session.GetString("UsuarioNombre");
    int idUsuario = BD.GetUsuarioId(usuario);
    var niveles = BD.ObtenerNiveles(idUsuario);
CargarDatosUsuario();

    return View(niveles);
}


public IActionResult Nivel(int id, int index = 0, int correctas = 0)
{
    var preguntas = BD.ObtenerPreguntasCamino(id);

    if (preguntas == null || preguntas.Count == 0)
        return RedirectToAction("Camino");

    // Cuando termina todas las preguntas
    if (index >= preguntas.Count)
    {
        int estrellas = 0;

        if (correctas >= 6) estrellas = 3;
        else if (correctas >= 4) estrellas = 2;
        else if (correctas >= 2) estrellas = 1;

        int idUsuario = BD.GetUsuarioId(HttpContext.Session.GetString("UsuarioNombre"));
        BD.GuardarResultadoCamino(idUsuario, id, estrellas);

        var fin = new NivelFinalViewModel
        {
            NumeroNivel = id,
            EstrellasObtenidas = estrellas
        };
CargarDatosUsuario();

        return View("NivelCompletado", fin);
    }

    // 👉 NIVEL INFO — para obtener EL NOMBRE real del nivel
    var nivelInfo = BD.ObtenerNiveles(BD.GetUsuarioId(
                       HttpContext.Session.GetString("UsuarioNombre")))
                       .FirstOrDefault(n => n.IDNivel == id);

    // 👉 Pregunta actual CORRECTA
    var p = preguntas[index];

    var vm = new NivelViewModel
    {
        NumeroNivel = id,
        TituloApp = nivelInfo?.NombreNivel ?? $"Nivel {id}",   // <-- NOMBRE REAL
        Pregunta = p.Enunciado,
        PorcentajeCompletado = (index * 100) / preguntas.Count,
        IndexPregunta = index,
        CorrectasHastaAhora = correctas,
        Opciones = new List<Opcion>
        {
            new Opcion { Texto = p.Opcion1, EsCorrecta = p.Correcta == 1 },
            new Opcion { Texto = p.Opcion2, EsCorrecta = p.Correcta == 2 },
            new Opcion { Texto = p.Opcion3, EsCorrecta = p.Correcta == 3 },
            new Opcion { Texto = p.Opcion4, EsCorrecta = p.Correcta == 4 }
        }
    };

    return View(vm);
}





        [HttpPost]
        public IActionResult GuardarResultado(int nivel, int estrellas)
        {
            int idUsuario = BD.GetUsuarioId(HttpContext.Session.GetString("UsuarioNombre"));
            BD.GuardarResultadoCamino(idUsuario, nivel, estrellas);

            return RedirectToAction("Camino");
        }
        // ---------------- ARTÍCULOS ----------------

public IActionResult Articulos(int pagina = 1)
{
    int pageSize = 9;
    int skip = (pagina - 1) * pageSize;

    var lista = BD.TraerArticulos()
                 .OrderByDescending(a => a.Fecha)
                 .ToList();

    int totalArticulos = lista.Count(); // ← corregido
    int totalPaginas = (int)Math.Ceiling(totalArticulos / (double)pageSize);

    var articulosPagina = lista
        .Skip(skip)
        .Take(pageSize)
        .ToList();

    ViewBag.PaginaActual = pagina;
    ViewBag.TotalPaginas = totalPaginas;

    return View(articulosPagina);
}


public IActionResult Articulo(int id)
{
    var art = BD.ObtenerArticuloPorId(id);
    if (art == null)
        return RedirectToAction("Articulos");

    return View(art);
}
// -------- ARTICULOS SOLO LOGUEADOS --------

public IActionResult ArticulosSesionado(int pagina = 1)
{
    if (!UsuarioLogueado())
        return RedirectToAction("IniciarSesion");

    int pageSize = 9;
    int skip = (pagina - 1) * pageSize;

    var lista = BD.TraerArticulos()
                  .OrderByDescending(a => a.Fecha)
                  .ToList();

    int totalArticulos = lista.Count();
    int totalPaginas = (int)Math.Ceiling(totalArticulos / (double)pageSize);

    var articulosPagina = lista
        .Skip(skip)
        .Take(pageSize)
        .ToList();

    ViewBag.PaginaActual = pagina;
    ViewBag.TotalPaginas = totalPaginas;
CargarDatosUsuario();

    return View("ArticulosSesionado", articulosPagina);
}

public IActionResult ArticuloSesionado(int id)
{
    if (!UsuarioLogueado())
        return RedirectToAction("IniciarSesion");

    var art = BD.ObtenerArticuloPorId(id);
    if (art == null)
        return RedirectToAction("ArticulosSesionado");
CargarDatosUsuario();

    return View("ArticuloSesionado", art);
}
private void CargarDatosUsuario()
{
    // Usuario logueado en sesión
    var nombreUsuario = HttpContext.Session.GetString("UsuarioNombre");
    if (string.IsNullOrEmpty(nombreUsuario)) return;

    // Lo buscamos en la BD
    var usuario = BD.ObtenerUsuarioPorNombre(nombreUsuario);
    if (usuario == null) return;

    // Estos nombres son los que vas a usar en la vista
    ViewBag.NombreUsuario   = usuario.NombreUsuario;
    ViewBag.Mail            = usuario.Mail;
    ViewBag.FechaNacimiento = usuario.FechaNacimiento.ToString("dd/MM/yyyy");

    ViewBag.FotoPerfil = string.IsNullOrEmpty(usuario.Foto)
        ? "/Imagenes/Perfil-Default.webp"
        : usuario.Foto;
}




[HttpPost]
public IActionResult CambiarFoto(IFormFile foto)
{
    if (foto == null || foto.Length == 0)
        return RedirectToAction("IndexSesionado");

    var nombreUsuario = HttpContext.Session.GetString("UsuarioNombre");

    // Crear carpeta si no existe
    var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imagenes/Perfiles");
    if (!Directory.Exists(carpeta))
        Directory.CreateDirectory(carpeta);

    var nombreArchivo = Guid.NewGuid() + Path.GetExtension(foto.FileName);
    var rutaFisica = Path.Combine(carpeta, nombreArchivo);

    // Guardar archivo
    using (var stream = new FileStream(rutaFisica, FileMode.Create))
        foto.CopyTo(stream);

    string rutaWeb = "/Imagenes/Perfiles/" + nombreArchivo;

    BD.ActualizarFotoUsuario(nombreUsuario, rutaWeb);

    return RedirectToAction("IndexSesionado");
}



    }
}
