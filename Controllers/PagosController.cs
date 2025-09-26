
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using ProyectoInmobiliariaADO.Data;
// using ProyectoInmobiliariaADO.Models;

// namespace ProyectoInmobiliariaADO.Controllers
// {
//     public class PagosController : Controller
//     {
//         private readonly RepositorioPago repoPago = new RepositorioPago();
//         private readonly RepositorioContrato repoContrato = new RepositorioContrato();

//         // GET: Pagos
//         public IActionResult Index()
//         {
//             var lista = repoPago.ObtenerTodos();
//             return View(lista);
//         }

//         // GET: Detalles del pago
//         public IActionResult Details(int id)
//         {
//             var pago = repoPago.ObtenerPorId(id);
//             if (pago == null) return NotFound();

//             var contrato = repoContrato.ObtenerPorId(pago.ContratoId);
//             ViewBag.Contrato = contrato != null ? $"Contrato {contrato.Id} - {contrato.InmuebleDireccion}" : "Desconocido";

//             return View(pago);
//         }

//         // GET: Crear pago
//         public IActionResult Create()
//         {
//             CargarContratos();
//             var mediosPago = new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" };
//             ViewBag.MediosPago = new SelectList(mediosPago);
//             return View();
//         }

//         // POST: Crear pago
//         [HttpPost]
//         [ValidateAntiForgeryToken]

//         public IActionResult Create(Pago pago)
//         {
//             if (ModelState.IsValid)
//             {
//                 // Validar que no exista ya un pago para el mismo contrato en el mismo mes/año
//                 var pagosExistentes = repoPago.ObtenerPorContrato(pago.ContratoId);
//                 bool existeMismoMes = pagosExistentes.Any(p =>
//                     p.Fecha.Year == pago.Fecha.Year && p.Fecha.Month == pago.Fecha.Month);

//                 if (existeMismoMes)
//                 {
//                     // Usamos ViewBag para mostrar el error en la misma vista
//                     ViewBag.Error = "Ya existe un pago registrado para este contrato en el mismo mes.";

//                     // Recargar combos
//                     CargarContratos(pago.ContratoId);
//                     var mediosPago = new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" };
//                     ViewBag.MediosPago = new SelectList(mediosPago, pago.MedioPago);

//                     return View(pago);
//                 }

//                 // Si todo está bien, se registra el pago
//                 repoPago.Alta(pago);
//                 TempData["PagoSuccess"] = "Pago registrado correctamente.";
//                 return RedirectToAction(nameof(Index));
//             }

//             // Si hay error de validación del modelo
//             CargarContratos(pago.ContratoId);
//             var medios = new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" };
//             ViewBag.MediosPago = new SelectList(medios, pago.MedioPago);
//             return View(pago);
//         }

//         // GET: Editar pago
//         public IActionResult Edit(int id)
//         {
//             var pago = repoPago.ObtenerPorId(id);
//             if (pago == null) return NotFound();

//             // Cargar contratos (si usas select)
//             CargarContratos(pago.ContratoId);

//             // Cargar medios de pago
//             var mediosPago = new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" };
//             ViewBag.MediosPago = new SelectList(mediosPago, pago.MedioPago);

//             return View(pago);
//         }

//         // POST: Editar pago
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public IActionResult Edit(Pago pago)
//         {
//             if (ModelState.IsValid)
//             {
//                 repoPago.Modificacion(pago);
//                 TempData["PagoSuccess"] = "Pago modificado correctamente.";
//                 return RedirectToAction(nameof(Index));
//             }

//             CargarContratos(pago.ContratoId);
//             return View(pago);
//         }



//         // POST: Eliminar pago
//         [HttpPost, ActionName("Anular")]
//         [ValidateAntiForgeryToken]
//         public IActionResult DeleteConfirmed(int id)
//         {
//             Console.WriteLine($"Intentando eliminar pago con id {id}"); // <-- debug
//             // Obtenemos el pago
//             var pago = repoPago.ObtenerPorId(id);
//             if (pago != null)
//             {
//                 // Cambiamos su estado a "Anulado"
//                 pago.Estado = "Anulado";

//                 // Guardamos la modificación
//                 repoPago.Modificacion(pago);

//                 TempData["PagoError"] = "Pago anulado correctamente.";
//             }
//             else
//             {
//                 TempData["PagoError"] = "No se encontró el pago a anular.";
//             }

//             return RedirectToAction(nameof(Index));
//         }

//         // --- Métodos privados ---
//         private void CargarContratos(int? contratoIdSeleccionado = null)
//         {
//             var contratos = repoContrato.ObtenerTodos()
//                             .Where(c => c.Estado == "Vigente") // solo contratos vigentes
//                             .Select(c => new
//                             {
//                                 c.Id,
//                                 Display = $"Contrato {c.Id} - {c.InmuebleDireccion} - {c.InquilinoNombre} {c.InquilinoApellido}"
//                             })
//                             .ToList();

//             ViewBag.Contratos = new SelectList(contratos, "Id", "Display", contratoIdSeleccionado);
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public IActionResult EliminarDefinitivo(int id)
//         {
//             // Verificar rol administrador
//             if (!User.IsInRole("Administrador"))
//             {
//                 return Forbid(); // 403 - No autorizado
//             }

//             try
//             {
//                 int res = repoPago.EliminarDefinitivo(id); // <-- aquí
//                 if (res > 0)
//                     TempData["PagoError"] = "Pago eliminado definitivamente.";
//                 else
//                     TempData["PagoError"] = "No se encontró el pago o no se pudo eliminar.";
//             }
//             catch (Exception ex)
//             {
//                 TempData["PagoError"] = $"Error al eliminar el pago: {ex.Message}";
//             }

//             return RedirectToAction("Index");
//         }

//     }
// }



// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using ProyectoInmobiliariaADO.Data;
// using ProyectoInmobiliariaADO.Models;
// using System.Linq;

// namespace ProyectoInmobiliariaADO.Controllers
// {
//     public class PagosController : Controller
//     {
//         private readonly RepositorioPago repoPago = new RepositorioPago();
//         private readonly RepositorioContrato repoContrato = new RepositorioContrato();

//         // GET: Pagos
//         public IActionResult Index()
//         {
//             var lista = repoPago.ObtenerTodos();
//             return View(lista);
//         }

//         // GET: Detalles del pago
//         public IActionResult Details(int id)
//         {
//             var pago = repoPago.ObtenerPorId(id);
//             if (pago == null) return NotFound();

//             var contrato = repoContrato.ObtenerPorId(pago.ContratoId);
//             ViewBag.Contrato = contrato != null ? $"Contrato {contrato.Id} - {contrato.InmuebleDireccion}" : "Desconocido";

//             return View(pago);
//         }

//         // GET: Crear pago
//         public IActionResult Create()
//         {
//             CargarContratos();
//             var mediosPago = new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" };
//             ViewBag.MediosPago = new SelectList(mediosPago);
//             return View();
//         }

//         // POST: Crear pago
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public IActionResult Create(Pago pago)
//         {
//             if (ModelState.IsValid)
//             {
//                 // Validar que no exista ya un pago para el mismo contrato en el mismo mes/año
//                 var pagosExistentes = repoPago.ObtenerPorContrato(pago.ContratoId);
//                 bool existeMismoMes = pagosExistentes.Any(p =>
//                     p.Fecha.Year == pago.Fecha.Year && p.Fecha.Month == pago.Fecha.Month);

//                 if (existeMismoMes)
//                 {
//                     ViewBag.Error = "Ya existe un pago registrado para este contrato en el mismo mes.";
//                     CargarContratos(pago.ContratoId);
//                     var mediosPago = new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" };
//                     ViewBag.MediosPago = new SelectList(mediosPago, pago.MedioPago);
//                     return View(pago);
//                 }

//                 // Registrar el pago con auditoría
//                 string usuario = User.Identity?.Name ?? "Sistema";
//                 repoPago.Alta(pago, usuario);

//                 TempData["PagoSuccess"] = "Pago registrado correctamente.";
//                 return RedirectToAction(nameof(Index));
//             }

//             // Error de validación
//             CargarContratos(pago.ContratoId);
//             var medios = new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" };
//             ViewBag.MediosPago = new SelectList(medios, pago.MedioPago);
//             return View(pago);
//         }

//         // GET: Editar pago
//         public IActionResult Edit(int id)
//         {
//             var pago = repoPago.ObtenerPorId(id);
//             if (pago == null) return NotFound();

//             CargarContratos(pago.ContratoId);
//             var mediosPago = new List<string> { "Efectivo", "Transferencia", "Tarjeta", "Cheque" };
//             ViewBag.MediosPago = new SelectList(mediosPago, pago.MedioPago);

//             return View(pago);
//         }

//         // POST: Editar pago
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public IActionResult Edit(Pago pago)
//         {
//             if (ModelState.IsValid)
//             {
//                 repoPago.Modificacion(pago);
//                 TempData["PagoSuccess"] = "Pago modificado correctamente.";
//                 return RedirectToAction(nameof(Index));
//             }

//             CargarContratos(pago.ContratoId);
//             return View(pago);
//         }

//         // POST: Anular pago
//         [HttpPost, ActionName("Anular")]
//         [ValidateAntiForgeryToken]
//         public IActionResult Anular(int id)
//         {
//             var pago = repoPago.ObtenerPorId(id);
//             if (pago != null)
//             {
//                 pago.Estado = "Anulado";
//                 string usuario = User.Identity?.Name ?? "Sistema";
//                 repoPago.Modificacion(pago);
//                 TempData["PagoError"] = "Pago anulado correctamente.";
//             }
//             else
//             {
//                 TempData["PagoError"] = "No se encontró el pago a anular.";
//             }

//             return RedirectToAction(nameof(Index));
//         }

//         // POST: Eliminar definitivo
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public IActionResult EliminarDefinitivo(int id)
//         {
//             if (!User.IsInRole("Administrador"))
//                 return Forbid();

//             try
//             {
//                 int res = repoPago.EliminarDefinitivo(id);
//                 TempData["PagoError"] = res > 0 ? "Pago eliminado definitivamente." : "No se encontró el pago o no se pudo eliminar.";
//             }
//             catch (Exception ex)
//             {
//                 TempData["PagoError"] = $"Error al eliminar el pago: {ex.Message}";
//             }

//             return RedirectToAction(nameof(Index));
//         }

//         // --- Métodos privados ---
//         private void CargarContratos(int? contratoIdSeleccionado = null)
//         {
//             var contratos = repoContrato.ObtenerTodos()
//                 .Where(c => c.Estado == "Vigente")
//                 .Select(c => new
//                 {
//                     c.Id,
//                     Display = $"Contrato {c.Id} - {c.InmuebleDireccion} - {c.InquilinoNombre} {c.InquilinoApellido}"
//                 })
//                 .ToList();

//             ViewBag.Contratos = new SelectList(contratos, "Id", "Display", contratoIdSeleccionado);
//         }
//     }
// }



using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;

namespace ProyectoInmobiliariaADO.Controllers
{
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
        public IActionResult Create(Pago pago)
        {
            if (ModelState.IsValid)
            {
                // Validar que no exista ya un pago para el mismo contrato en el mismo mes/año
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
        // POST: Pagos/Anular/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Anular(int id)
        {
            // Verificar que el pago exista
            var pago = repoPago.ObtenerPorId(id);
            if (pago == null)
            {
                TempData["PagoError"] = "No se encontró el pago a anular.";
                return RedirectToAction(nameof(Index));
            }

            // Obtener el usuario actual
            string usuarioActual = User.Identity?.Name ?? "Desconocido";

            try
            {
                // Llamamos al repositorio para anular pasando id y usuario
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
        public IActionResult EliminarDefinitivo(int id)
        {
            if (!User.IsInRole("Administrador"))
            {
                return Forbid();
            }

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
