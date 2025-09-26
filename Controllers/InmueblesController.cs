
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
        private readonly RepositorioTipoInmueble repoTipo = new RepositorioTipoInmueble();

        // GET: Index con filtros opcionales
        public IActionResult Index(string filtro = "Todos", DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var lista = repo.ObtenerTodos();

            // Filtrar disponibles por estado
            if (filtro == "Disponibles")
            {
                lista = lista.Where(i => i.Estado == "Disponible").ToList();
            }

            // Filtrar por disponibilidad entre fechas
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                lista = repo.ObtenerTodos();
                lista = repo.ObtenerDisponiblesEntreFechas(fechaInicio.Value, fechaFin.Value);
                ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
                ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");
                filtro = "PorFechas";
            }

            // Ordenar por estado
            var estadosOrden = new List<string> { "Alquilado", "Disponible" };
            lista = lista.OrderBy(i => estadosOrden.IndexOf(i.Estado)).ToList();

            ViewBag.Filtro = filtro;
            return View(lista);
        }

        // GET: Detalles de inmueble
        public IActionResult Details(int id)
        {
            var m = repo.ObtenerPorId(id);
            if (m == null) return NotFound();
            return View(m);
        }

        // GET: Crear inmueble
        public IActionResult Create()
        {
            CargarPropietarios();
            CargarTipos();
            return View();
        }

        // POST: Crear inmueble
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                inmueble.Estado = "Disponible";
                repo.Alta(inmueble);
                return RedirectToAction(nameof(Index));
            }
            CargarPropietarios(inmueble.PropietarioId);
            CargarTipos(inmueble.TipoId);
            return View(inmueble);
        }

        // GET: Editar inmueble
        public IActionResult Edit(int id)
        {
            var m = repo.ObtenerPorId(id);
            if (m == null) return NotFound();
            CargarPropietarios(m.PropietarioId);
            CargarTipos(m.TipoId);
            return View(m);
        }

        // POST: Editar inmueble
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
            CargarTipos(inmueble.TipoId);
            return View(inmueble);
        }

        // GET: Eliminar inmueble
        public IActionResult Delete(int id)
        {
            var inmueble = repo.ObtenerPorId(id);
            if (inmueble == null) return NotFound();

            var contratosRepo = new RepositorioContrato();
            var contratos = contratosRepo.ObtenerContratosPorInmueble(id);

            if (contratos.Count > 0 || inmueble.Estado == "Alquilado" || inmueble.Estado == "Reservado")
            {
                TempData["ErrorEliminar"] = "No se puede eliminar un inmueble que tenga contratos o esté alquilado/reservado.";
                return RedirectToAction(nameof(Index));
            }

            repo.Baja(id);
            TempData["EliminarSuccess"] = "Inmueble eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Confirmar eliminación
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // Cargar lista de propietarios para dropdown
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

        // Cargar lista de tipos activos para dropdown
        private void CargarTipos(int? tipoIdSeleccionado = null)
        {
            var tipos = repoTipo.ObtenerTodosActivos();
            ViewBag.Tipos = new SelectList(tipos, "Id", "Descripcion", tipoIdSeleccionado);
        }
    }
}
