using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;
using System.Linq;

namespace ProyectoInmobiliariaADO.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder a este controlador
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilino repo;

        public InquilinosController()
        {
            repo = new RepositorioInquilino();
        }

        // Listar todos los inquilinos
        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // Crear inquilino (GET)
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create()
        {
            CargarInquilinos();
            return View();
        }

        // Crear inquilino (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create(Inquilino inquilino)
        {
            if (repo.ObtenerPorDNI(inquilino.DNI) != null)
            {
                ModelState.AddModelError("DNI", "El DNI ya está registrado para otro inquilino.");
                CargarInquilinos(inquilino.Id);
                return View(inquilino);
            }

            if (ModelState.IsValid)
            {
                repo.Alta(inquilino);
                return RedirectToAction(nameof(Index));
            }

            CargarInquilinos(inquilino.Id);
            return View(inquilino);
        }

        // Editar inquilino (GET)
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(int id)
        {
            var i = repo.ObtenerPorId(id);
            if (i == null) return NotFound();
            CargarInquilinos(i.Id);
            return View(i);
        }

        // Editar inquilino (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(int id, Inquilino inquilino)
        {
            if (id != inquilino.Id) return NotFound();

            var existente = repo.ObtenerPorDNI(inquilino.DNI);
            if (existente != null && existente.Id != inquilino.Id)
            {
                ModelState.AddModelError("DNI", "El DNI ya está registrado para otro inquilino.");
                CargarInquilinos(inquilino.Id);
                return View(inquilino);
            }

            if (ModelState.IsValid)
            {
                repo.Modificacion(inquilino);
                return RedirectToAction(nameof(Index));
            }

            CargarInquilinos(inquilino.Id);
            return View(inquilino);
        }

        // Eliminar inquilino (GET)
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var i = repo.ObtenerPorId(id);
            if (i == null) return NotFound();
            return View(i);
        }

        // Eliminar inquilino (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // Detalles de inquilino
        public IActionResult Details(int id)
        {
            var i = repo.ObtenerPorId(id);
            if (i == null) return NotFound();
            return View(i);
        }

        // Método privado para cargar dropdown con Apellido, Nombre y DNI
        private void CargarInquilinos(int? inquilinoIdSeleccionado = null)
        {
            var inquilinos = repo.ObtenerTodos()
                                 .Select(i => new
                                 {
                                     i.Id,
                                     Display = $"{i.Apellido}, {i.Nombre} - DNI: {i.DNI}"
                                 })
                                 .ToList();

            ViewBag.Inquilinos = new SelectList(inquilinos, "Id", "Display", inquilinoIdSeleccionado);
        }
    }
}
