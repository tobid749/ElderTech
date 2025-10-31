using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;

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
        public IActionResult CrearArticulo(Articulo articulo, IFormFile? FotoFile)
        {
            // Autor lo cargan en el formulario (si está vacío, fallback a Session)
            if (string.IsNullOrWhiteSpace(articulo.AutorNombre))
                articulo.AutorNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Invitado";

            articulo.Fecha = DateTime.Now;

            if (FotoFile != null && FotoFile.Length > 0)
            {
                using var ms = new MemoryStream();
                FotoFile.CopyTo(ms);
                articulo.Foto = ms.ToArray();
                articulo.FotoContentType = FotoFile.ContentType;
            }

            var id = BD.InsertArticulo(articulo);
            return RedirectToAction("Detalle", new { id });
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var art = BD.GetArticuloById(id);
            if (art == null) return NotFound();
            return View(art);
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var art = BD.GetArticuloById(id);
            if (art == null) return NotFound();
            return View(art);
        }

        [HttpPost]
        public IActionResult Editar(Articulo articulo, IFormFile? NuevaFoto)
        {
            if (string.IsNullOrWhiteSpace(articulo.AutorNombre))
                articulo.AutorNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Invitado";

            if (NuevaFoto != null && NuevaFoto.Length > 0)
            {
                using var ms = new MemoryStream();
                NuevaFoto.CopyTo(ms);
                articulo.Foto = ms.ToArray();
                articulo.FotoContentType = NuevaFoto.ContentType;
            }
            else
            {
                // No sobreescribir imagen si no envían una nueva
                articulo.Foto = null;
                articulo.FotoContentType = null;
            }

            if (articulo.Fecha == default) articulo.Fecha = DateTime.Now;

            BD.UpdateArticulo(articulo);
            return RedirectToAction("Detalle", new { id = articulo.IDArticulo });
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            BD.DeleteArticulo(id);
            return RedirectToAction("Lista");
        }

        [HttpGet]
        public IActionResult Lista()
        {
            var arts = BD.GetArticulos();
            return View(arts);
        }
    }
}
