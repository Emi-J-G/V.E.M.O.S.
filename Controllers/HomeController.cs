using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VEMOS.Models;

namespace VEMOS.Controllers;

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

    [HttpPost]
    public IActionResult Registro()
    {
        Usuario usuario = new Usuario();
        DB db = new DB();
        db.registrarUsuario(usuario);
        return View();
    }

    [HttpPost]
    public IActionResult Login(string nombreUsuario, string contraseña) {
        Usuario usuario = new Usuario();
        DB db = new DB();
        //se fija si usuario existe
        usuario = db.usuarioExiste(nombreUsuario);
        usuario = db.validarUsuario(usuario.email, contraseña);
        if (usuario != null) {
            //usuario logueado correctamente
            ViewBag.Mensaje = "Bienvenido, " + usuario.nombre + "!";
        } else {
            //usuario no encontrado
            ViewBag.Mensaje = "Usuario o contraseña incorrectos.";
        }
        return View("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
