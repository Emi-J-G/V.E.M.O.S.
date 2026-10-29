using Microsoft.AspNetCore.Mvc;

namespace VEMOS.Controllers;

public class OptionsController : Controller
{
    public IActionResult Perfil()
    {
        return View();
    }

    public IActionResult Ajustes()
    {
        return View();
    }

    public IActionResult Amigos()
    {
        return View();
    }

    public IActionResult Reportar()
    {
        return View();
    }
}
