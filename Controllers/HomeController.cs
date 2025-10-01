using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliariaADO.Models;

namespace ProyectoInmobiliariaADO.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder al Home
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Vista principal accesible para todos los usuarios logueados (empleados y administradores)
        public IActionResult Index()
        {
            ViewData["Message"] = "Bienvenido a la Inmobiliaria. Use el menú o los botones abajo para navegar.";
            return View();
        }

        // Política general de privacidad, también accesible para usuarios autenticados
        public IActionResult Privacy()
        {
            return View();
        }

        // Página de error, accesible para cualquier persona (no requiere login)
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
