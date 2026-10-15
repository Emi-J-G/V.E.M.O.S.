using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VEMOS.Models;

namespace VEMOS.Controllers;

public class LoginRegisController : Controller
{
    private readonly ILogger<LoginRegisController> _logger;

    public LoginRegisController(ILogger<LoginRegisController> logger)
    {
        _logger = logger;
    }


    [HttpPost]
    public IActionResult Registro(string nombreUsuario, string contrasena, string apellido, string email, string[] restricciones)
    {
        Usuario usuario = new Usuario()
        {
            username = nombreUsuario,
            nombre = nombreUsuario,
            apellido = apellido,
            email = email,
            contrasena = contrasena,
            restricciones = restricciones.Select(r => new Restriccion { nombre = r }).ToList()
        };
        DB db = new DB();
        db.registrarUsuario(usuario);
        HttpContext.Session.SetString("Usuario", usuario.username);
        HttpContext.Session.SetString("NombreCompleto", usuario.nombre);
        return View();
    }

    [HttpPost]
    public IActionResult Login(string nombreUsuario, string contrasena) {
        DB db = new DB();
        //se fija si usuario existe
        Usuario usuario = db.usuarioExiste(nombreUsuario);
        if (usuario != null)
        {
            usuario = db.validarUsuario(usuario.email, contrasena);
        }

        if (usuario != null) {
            //usuario logueado correctamente
            HttpContext.Session.SetString("Usuario", nombreUsuario);
            HttpContext.Session.SetString("NombreCompleto", nombreUsuario);
            ViewBag.Mensaje = "Bienvenido, " + usuario.nombre + "!";
            return RedirectToAction("Index", "Home");
        }
        else {
            //usuario no encontrado
            ViewBag.Mensaje = "Usuario o contraseña incorrectos.";
        }

        return View("~/Views/Home/Index.cshtml");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Index()
    {
        return View();
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
