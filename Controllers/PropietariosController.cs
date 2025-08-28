using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;

namespace ProyectoInmobiliariaADO.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly RepositorioPropietario repo = new RepositorioPropietario();

        // GET: Propietarios
        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Propietarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                // Validar que no exista DNI duplicado
                var existente = repo.ObtenerPorDNI(propietario.DNI);
                if (existente != null)
                {
                    ModelState.AddModelError("DNI", "Ya existe un propietario con ese DNI.");
                    return View(propietario);
                }

                repo.Alta(propietario);
                return RedirectToAction(nameof(Index));
            }
            return View(propietario);
        }

        // GET: Propietarios/Edit/5
        public IActionResult Edit(int id)
        {
            var propietario = repo.ObtenerPorId(id);
            if (propietario == null) return NotFound();
            return View(propietario);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                // Validar que el DNI no esté duplicado en otro registro
                var existente = repo.ObtenerPorDNI(propietario.DNI);
                if (existente != null && existente.Id != propietario.Id)
                {
                    ModelState.AddModelError("DNI", "Ya existe un propietario con ese DNI.");
                    return View(propietario);
                }

                repo.Modificacion(propietario);
                return RedirectToAction(nameof(Index));
            }
            return View(propietario);
        }

        // GET: Propietarios/Delete/5
        public IActionResult Delete(int id)
        {
            var propietario = repo.ObtenerPorId(id);
            if (propietario == null) return NotFound();
            return View(propietario);
        }

        // POST: Propietarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Propietarios/Details/5
        public IActionResult Details(int id)
        {
            var propietario = repo.ObtenerPorId(id);
            if (propietario == null) return NotFound();
            return View(propietario);
        }
    }
}
