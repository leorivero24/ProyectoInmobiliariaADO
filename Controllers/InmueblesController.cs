using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;

namespace ProyectoInmobiliariaADO.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly RepositorioInmueble repo = new RepositorioInmueble();
        private readonly RepositorioPropietario repoPropietario = new RepositorioPropietario();

        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            // Orden personalizado por Estado
            var estadosOrden = new List<string> { "Alquilado", "Reservado", "Disponible" };
            lista = lista.OrderBy(i => estadosOrden.IndexOf(i.Estado)).ToList();
            return View(lista);
        }

        public IActionResult Details(int id)
        {
            var m = repo.ObtenerPorId(id);
            if (m == null) return NotFound();
            return View(m);
        }

        // GET: Inmuebles/Create
        public IActionResult Create()
        {
            CargarPropietarios();
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                repo.Alta(inmueble);
                return RedirectToAction(nameof(Index));
            }
            CargarPropietarios();
            return View(inmueble);
        }

        // GET: Inmuebles/Edit/5
        public IActionResult Edit(int id)
        {
            var m = repo.ObtenerPorId(id);
            if (m == null) return NotFound();

            CargarPropietarios(m.PropietarioId);
            return View(m);
        }

        // POST: Inmuebles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                repo.Modificacion(inmueble);
                return RedirectToAction(nameof(Index));
            }
            CargarPropietarios(inmueble.PropietarioId);
            return View(inmueble);
        }

        // GET: Inmuebles/Delete/5
        public IActionResult Delete(int id)
        {
            var inmueble = repo.ObtenerPorId(id);
            if (inmueble == null) return NotFound();

            // Revisar si tiene contratos asociados
            var contratosRepo = new RepositorioContrato();
            var contratos = contratosRepo.ObtenerContratosPorInmueble(id);

            if (contratos.Count > 0)
            {
                // Guardamos mensaje en TempData para mostrarlo en la vista Index
                TempData["ErrorEliminar"] = "No se puede eliminar el inmueble porque tiene contratos asociados.";
                return RedirectToAction(nameof(Index));
            }

            if (inmueble.Estado == "Alquilado" || inmueble.Estado == "Reservado")
            {
                TempData["ErrorEliminar"] = "No se puede eliminar un inmueble que esté alquilado o reservado.";
                return RedirectToAction(nameof(Index));
            }

            // Si no tiene contratos, se elimina
            repo.Baja(id);
            TempData["Success"] = "Inmueble eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Inmuebles/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // Método privado para cargar lista de propietarios
        private void CargarPropietarios(int? propietarioIdSeleccionado = null)
        {
            var propietarios = repoPropietario.ObtenerTodos()
                                .Select(p => new
                                {
                                    p.Id,
                                    NombreCompleto = $"{p.Apellido}, {p.Nombre}"
                                });

            ViewBag.Propietarios = new SelectList(propietarios, "Id", "NombreCompleto", propietarioIdSeleccionado);
        }
    }
}
