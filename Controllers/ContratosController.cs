using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;
using ProyectoInmobiliariaADO.Models.ViewModels;
using System;
using System.Linq;

namespace ProyectoInmobiliariaADO.Controllers
{
    [Authorize] 
    public class ContratosController : Controller
    {
        private readonly RepositorioContrato repo = new RepositorioContrato();
        private readonly RepositorioInmueble repoInm = new RepositorioInmueble();
        private readonly RepositorioInquilino repoInq = new RepositorioInquilino();

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
                Inquilino = repoInq.ObtenerPorId(c.InquilinoId)!
            };
            return View(vm);
        }

        // --- Crear Contrato ---
        [Authorize(Roles = "Administrador,Empleado")] // Solo ciertos roles pueden crear contratos
        public IActionResult Create()
        {
            CargarInmuebles();
            CargarInquilinos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create(Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                CargarInmuebles(contrato.InmuebleId);
                CargarInquilinos(contrato.InquilinoId);
                return View(contrato);
            }

            try
            {
                contrato.Estado = "Vigente";
                string usuario = User.Identity?.Name ?? "Desconocido";
                repo.Alta(contrato, usuario);

                var inmueble = repoInm.ObtenerPorId(contrato.InmuebleId);
                if (inmueble != null)
                {
                    inmueble.Estado = "Alquilado";
                    repoInm.Modificacion(inmueble);
                }

                TempData["ContratoSuccess"] = "Contrato creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CargarInmuebles(contrato.InmuebleId);
                CargarInquilinos(contrato.InquilinoId);
                return View(contrato);
            }
        }

        // --- Editar Contrato ---
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            CargarInmuebles(contrato.InmuebleId);
            CargarInquilinos(contrato.InquilinoId);
            return View(contrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                CargarInmuebles(contrato.InmuebleId);
                CargarInquilinos(contrato.InquilinoId);
                return View(contrato);
            }

            try
            {
                repo.Modificacion(contrato);

                var inmueble = repoInm.ObtenerPorId(contrato.InmuebleId);
                if (inmueble != null)
                {
                    if (contrato.Estado == "Finalizado" || contrato.Estado == "Rescindido")
                    {
                        var otrosVigentes = repo.ObtenerContratosPorInmueble(inmueble.Id)
                                                .Any(c => c.Estado == "Vigente");
                        inmueble.Estado = otrosVigentes ? "Alquilado" : "Disponible";
                        repoInm.Modificacion(inmueble);
                    }
                    else if (contrato.Estado == "Vigente")
                    {
                        inmueble.Estado = "Alquilado";
                        repoInm.Modificacion(inmueble);
                    }
                }

                TempData["ContratoSuccess"] = "Contrato modificado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CargarInmuebles(contrato.InmuebleId);
                CargarInquilinos(contrato.InquilinoId);
                return View(contrato);
            }
        }

        // --- Anular Contrato ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Anular(int id)
        {
            try
            {
                string usuario = User.Identity?.Name ?? "Desconocido";
                repo.Anular(id, usuario);

                var contrato = repo.ObtenerPorId(id);
                if (contrato != null)
                {
                    var inmueble = repoInm.ObtenerPorId(contrato.InmuebleId);
                    if (inmueble != null)
                    {
                        var otrosVigentes = repo.ObtenerContratosPorInmueble(inmueble.Id)
                                                .Any(c => c.Estado == "Vigente");
                        inmueble.Estado = otrosVigentes ? "Alquilado" : "Disponible";
                        repoInm.Modificacion(inmueble);
                    }
                }

                TempData["ContratoSuccess"] = "Contrato anulado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ContratoError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // --- Renovar Contrato ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Renovar(int id, DateTime nuevaFechaInicio, DateTime? nuevaFechaFin, decimal nuevoMonto)
        {
            var contrato = repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            try
            {
                string usuario = User.Identity?.Name ?? "Desconocido";
                repo.Renovar(contrato, nuevaFechaInicio, nuevaFechaFin, nuevoMonto, usuario);

                var inmueble = repoInm.ObtenerPorId(contrato.InmuebleId);
                if (inmueble != null)
                {
                    inmueble.Estado = "Alquilado";
                    repoInm.Modificacion(inmueble);
                }

                TempData["ContratoSuccess"] = "Contrato renovado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ContratoError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // --- Eliminar definitivo ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")] // Solo Admin puede eliminar definitivamente
        public IActionResult EliminarDefinitivo(int id)
        {
            try
            {
                repo.EliminarDefinitivo(id);
                TempData["ContratoSuccess"] = "Contrato eliminado permanentemente.";
            }
            catch (Exception ex)
            {
                TempData["ContratoError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // --- Métodos privados ---
        private void CargarInquilinos(int? inquilinoIdSeleccionado = null)
        {
            var inquilinos = repoInq.ObtenerTodos() ?? new System.Collections.Generic.List<Inquilino>();
            var lista = inquilinos
                        .Select(i => new { i.Id, Display = $"{i.Nombre} {i.Apellido} - DNI: {i.DNI}" })
                        .ToList();
            ViewBag.Inquilinos = new SelectList(lista, "Id", "Display", inquilinoIdSeleccionado);
        }

        private void CargarInmuebles(int? inmuebleIdSeleccionado = null)
        {
            var inmuebles = repoInm.ObtenerTodos() ?? new System.Collections.Generic.List<Inmueble>();
            var lista = inmuebles
                        .Where(x => x.Estado == "Disponible" || x.Id == inmuebleIdSeleccionado)
                        .Select(i => new
                        {
                            i.Id,
                            Display = $"{i.Direccion} - {(i.Tipo != null ? i.Tipo.Descripcion : "Sin tipo")}"
                        })
                        .ToList();

            ViewBag.Inmuebles = new SelectList(lista, "Id", "Display", inmuebleIdSeleccionado);
        }
    }
}
