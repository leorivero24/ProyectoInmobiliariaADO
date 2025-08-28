using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;

namespace ProyectoInmobiliariaADO.Controllers
{
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
        public IActionResult Create()
        {
            return View();
        }

        // Crear inquilino (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inquilino inquilino)
        {
            if (repo.ObtenerPorDNI(inquilino.DNI) != null)
            {
                ModelState.AddModelError("DNI", "El DNI ya está registrado para otro inquilino.");
                return View(inquilino);
            }

            if (ModelState.IsValid)
            {
                repo.Alta(inquilino); // INSERT
                return RedirectToAction(nameof(Index));
            }
            return View(inquilino);
        }

        // Editar inquilino (GET)
        public IActionResult Edit(int id)
        {
            var i = repo.ObtenerPorId(id);
            if (i == null) return NotFound();
            return View(i);
        }

        // Editar inquilino (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Inquilino inquilino)
        {
            if (id != inquilino.Id) return NotFound();

            // Validar que el DNI no exista en otro inquilino
            var existente = repo.ObtenerPorDNI(inquilino.DNI);
            if (existente != null && existente.Id != inquilino.Id)
            {
                ModelState.AddModelError("DNI", "El DNI ya está registrado para otro inquilino.");
                return View(inquilino);
            }

            if (ModelState.IsValid)
            {
                repo.Modificacion(inquilino); // UPDATE seguro
                return RedirectToAction(nameof(Index));
            }

            return View(inquilino);
        }

        // Eliminar inquilino (GET)
        public IActionResult Delete(int id)
        {
            var i = repo.ObtenerPorId(id);
            if (i == null) return NotFound();
            return View(i);
        }

        // Eliminar inquilino (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id); // DELETE
            return RedirectToAction(nameof(Index));
        }

        // Detalles de inquilino
        public IActionResult Details(int id)
        {
            var i = repo.ObtenerPorId(id);
            if (i == null) return NotFound();
            return View(i);
        }
    }
}
