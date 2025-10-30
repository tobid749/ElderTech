using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Eldertech.Models;

namespace Eldertech.Controllers;

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
        return View();
    }
    public IActionResult IniciarSesion()
    {
        return View();
    }
    public IActionResult Registrarse()
    {
        return View();
    }
    public IActionResult RecuperarContraseña()
    {
        return View();
    }
     public IActionResult Mail()
    {
        return View();
    }
         public IActionResult Articulos()
    {
        return View();
    }
    public IActionResult Articulos2()
{
    return View();
}
}
