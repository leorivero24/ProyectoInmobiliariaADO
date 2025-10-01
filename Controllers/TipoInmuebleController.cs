using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliariaADO.Models;
using ProyectoInmobiliariaADO.Data;

namespace ProyectoInmobiliariaADO.Controllers
{
    [Authorize] // Solo usuarios autenticados
    public class TipoInmuebleController : Controller
    {
        private readonly RepositorioTipoInmueble repo = new RepositorioTipoInmueble();

        // GET: TipoInmueble
        public IActionResult Index()
        {
            var lista = repo.ObtenerTodosActivos();
            return View(lista);
        }

        // GET: TipoInmueble/Details/5
        public IActionResult Details(int id)
        {
            var tipo = repo.ObtenerPorId(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        // GET: TipoInmueble/Create
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoInmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create(TipoInmueble tipo)
        {
            if (ModelState.IsValid)
            {
                repo.Alta(tipo);
                TempData["Success"] = "Tipo de inmueble creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(tipo);
        }

        // GET: TipoInmueble/Edit/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(int id)
        {
            var tipo = repo.ObtenerPorId(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        // POST: TipoInmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(TipoInmueble tipo)
        {
            if (ModelState.IsValid)
            {
                repo.Modificacion(tipo);
                TempData["Success"] = "Tipo de inmueble modificado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(tipo);
        }

        // GET: TipoInmueble/Delete/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var tipo = repo.ObtenerPorId(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        // POST: TipoInmueble/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            var tipo = repo.ObtenerPorId(id);
            if (tipo == null) return NotFound();

            if (tipo.Estado == "Inactivo")
            {
                TempData["Error"] = "El tipo ya está inactivo.";
            }
            else
            {
                repo.Baja(id);
                TempData["Success"] = "Tipo de inmueble eliminado correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
