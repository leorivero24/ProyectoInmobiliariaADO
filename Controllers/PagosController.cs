using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;
using System.Linq;

namespace ProyectoInmobiliariaADO.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder
    public class PagosController : Controller
    {
        private readonly RepositorioPago repoPago = new RepositorioPago();
        private readonly RepositorioContrato repoContrato = new RepositorioContrato();

        // GET: Pagos
        public IActionResult Index()
        {
            var lista = repoPago.ObtenerTodos();
            return View(lista);
        }

        // GET: Detalles de un pago
        public IActionResult Details(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            if (pago == null) return NotFound();

            var contrato = repoContrato.ObtenerPorId(pago.ContratoId);
            ViewBag.Contrato = contrato != null ? $"Contrato {contrato.Id} - {contrato.InmuebleDireccion}" : "Desconocido";

            return View(pago);
        }

        // GET: Crear pago
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create()
        {
            CargarContratos();
            var mediosPago = new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" };
            ViewBag.MediosPago = new SelectList(mediosPago);
            return View();
        }

        // POST: Crear pago
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create(Pago pago)
        {
            if (ModelState.IsValid)
            {
                var pagosExistentes = repoPago.ObtenerTodos().Where(p => p.ContratoId == pago.ContratoId);
                bool existeMismoMes = pagosExistentes.Any(p => p.Fecha.Year == pago.Fecha.Year && p.Fecha.Month == pago.Fecha.Month);

                if (existeMismoMes)
                {
                    ViewBag.Error = "Ya existe un pago registrado para este contrato en el mismo mes.";
                    CargarContratos(pago.ContratoId);
                    ViewBag.MediosPago = new SelectList(new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" }, pago.MedioPago);
                    return View(pago);
                }

                string usuario = User.Identity?.Name ?? "Desconocido";
                repoPago.Alta(pago, usuario);

                TempData["PagoSuccess"] = "Pago registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            CargarContratos(pago.ContratoId);
            ViewBag.MediosPago = new SelectList(new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" }, pago.MedioPago);
            return View(pago);
        }

        // GET: Editar pago
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            if (pago == null) return NotFound();

            CargarContratos(pago.ContratoId);
            ViewBag.MediosPago = new SelectList(new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" }, pago.MedioPago);
            return View(pago);
        }

        // POST: Editar pago
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(Pago pago)
        {
            if (ModelState.IsValid)
            {
                repoPago.Modificacion(pago);
                TempData["PagoSuccess"] = "Pago modificado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            CargarContratos(pago.ContratoId);
            ViewBag.MediosPago = new SelectList(new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" }, pago.MedioPago);
            return View(pago);
        }

        // POST: Anular pago
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Anular(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            if (pago == null)
            {
                TempData["PagoError"] = "No se encontró el pago a anular.";
                return RedirectToAction(nameof(Index));
            }

            string usuarioActual = User.Identity?.Name ?? "Desconocido";

            try
            {
                repoPago.Anular(id, usuarioActual);
                TempData["PagoSuccess"] = "Pago anulado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["PagoError"] = $"Error al anular el pago: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Eliminar definitivo (solo admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarDefinitivo(int id)
        {
            try
            {
                int res = repoPago.EliminarDefinitivo(id);
                TempData["PagoError"] = res > 0 ? "Pago eliminado definitivamente." : "No se pudo eliminar el pago.";
            }
            catch (Exception ex)
            {
                TempData["PagoError"] = $"Error al eliminar el pago: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // --- Métodos privados ---
        private void CargarContratos(int? contratoIdSeleccionado = null)
        {
            var contratos = repoContrato.ObtenerTodos()
                .Where(c => c.Estado == "Vigente")
                .Select(c => new
                {
                    c.Id,
                    Display = $"Contrato {c.Id} - {c.InmuebleDireccion} - {c.InquilinoNombre} {c.InquilinoApellido}"
                }).ToList();

            ViewBag.Contratos = new SelectList(contratos, "Id", "Display", contratoIdSeleccionado);
        }
    }
}
