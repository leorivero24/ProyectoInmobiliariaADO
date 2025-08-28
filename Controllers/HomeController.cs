using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliariaADO.Models;

namespace ProyectoInmobiliariaADO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Puedes pasar datos a la vista si quieres mostrar mensajes o links
            ViewData["Message"] = "Bienvenido a la Inmobiliaria. Use el menú o los botones abajo para navegar.";
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
}
