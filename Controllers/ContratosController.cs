using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;
using ProyectoInmobiliariaADO.Models.ViewModels;

namespace ProyectoInmobiliariaADO.Controllers
{
    public class ContratosController : Controller
    {
        private readonly RepositorioContrato repo = new RepositorioContrato();
        private readonly RepositorioInmueble repoInm = new RepositorioInmueble();
        private readonly RepositorioInquilino repoInq = new RepositorioInquilino();
        private readonly RepositorioPago repoPago = new RepositorioPago(); // opcional

        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Details(int id)
        {
            var c = repo.ObtenerPorId(id);
            if (c == null) return NotFound();

            var vm = new ContratoDetailsViewModel
            {
                Contrato = c,
                Inmueble = repoInm.ObtenerPorId(c.InmuebleId)!,
                Inquilino = repoInq.ObtenerPorId(c.InquilinoId)!,
                Pagos = repoPago?.ObtenerPorContrato(c.Id)
            };
            return View(vm);
        }

        // GET: Crear contrato
        public IActionResult Create()
        {
            CargarInmuebles();
            CargarInquilinos();
            return View();
        }

        // POST: Crear contrato
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                repo.Alta(contrato);
                TempData["Success"] = "Contrato creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Recargar listas si hay error de validación
            CargarInmuebles(contrato.InmuebleId);
            CargarInquilinos(contrato.InquilinoId);
            return View(contrato);
        }

        // GET: Editar contrato
        public IActionResult Edit(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            CargarInmuebles(contrato.InmuebleId);
            CargarInquilinos(contrato.InquilinoId);
            return View(contrato);
        }

        // POST: Editar contrato
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                repo.Modificacion(contrato);
                TempData["Success"] = "Contrato modificado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Recargar listas si hay error de validación
            CargarInmuebles(contrato.InmuebleId);
            CargarInquilinos(contrato.InquilinoId);
            return View(contrato);
        }

        // GET: Eliminar contrato
        public IActionResult Delete(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            var inquilino = repoInq.ObtenerPorId(contrato.InquilinoId);
            var inmueble = repoInm.ObtenerPorId(contrato.InmuebleId);

            ViewBag.Inquilino = inquilino != null ? $"{inquilino.Nombre} {inquilino.Apellido}" : "Desconocido";
            ViewBag.Inmueble = inmueble != null ? $"{inmueble.Direccion} - {inmueble.Tipo}" : "Desconocido";

            return View(contrato);
        }

        // POST: Eliminar contrato
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            if (!contrato.FechaFin.HasValue || contrato.FechaFin.Value >= DateTime.Today)
            {
                TempData["Error"] = "No se puede eliminar el contrato porque está vigente o asociado a un inmueble.";
                return RedirectToAction(nameof(Index));
            }

            repo.Baja(id);
            TempData["Success"] = "Contrato eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // --- Métodos privados ---
        private void CargarInquilinos(int? inquilinoIdSeleccionado = null)
        {
            var inquilinos = repoInq.ObtenerTodos() ?? new List<Inquilino>(); // asegura que nunca sea null
            Console.WriteLine("Inquilinos count: " + inquilinos.Count); // <-- revisa esto
            var lista = inquilinos
                        .Select(i => new
                        {
                            i.Id,
                            Display = $"{i.Nombre} {i.Apellido} - DNI: {i.DNI}"
                        })
                        .ToList(); // ahora lista nunca es null
            ViewBag.Inquilinos = new SelectList(lista, "Id", "Display", inquilinoIdSeleccionado);
        }

        private void CargarInmuebles(int? inmuebleIdSeleccionado = null)
        {
            var inmuebles = repoInm.ObtenerTodos() ?? new List<Inmueble>(); // asegura que nunca sea null
            var lista = inmuebles
                        .Where(x => x.Estado == "Disponible")
                        .Select(i => new
                        {
                            i.Id,
                            Display = $"{i.Direccion} - {i.Tipo}"
                        })
                        .ToList(); // ahora lista nunca es null
            ViewBag.Inmuebles = new SelectList(lista, "Id", "Display", inmuebleIdSeleccionado);
        }

    }
}
