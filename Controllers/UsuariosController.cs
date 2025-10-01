using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;
using System.Security.Claims;

namespace ProyectoInmobiliariaADO.Controllers
{
    [Authorize] // Todos los métodos requieren usuario autenticado
    public class UsuariosController : Controller
    {
        private readonly RepositorioUsuario repo = new RepositorioUsuario();

        // GET: Usuarios
        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Detalles
        [Authorize(Roles = "Administrador")]
        public IActionResult Details(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // GET: Crear
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(Enum.GetValues<RolUsuario>());
            return View();
        }

        // POST: Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create(Usuario usuario, IFormFile? AvatarFile)
        {
            if (ModelState.IsValid)
            {
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);

                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AvatarFile.FileName)}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                    using var stream = new FileStream(path, FileMode.Create);
                    AvatarFile.CopyTo(stream);
                    usuario.Avatar = fileName;
                }

                repo.Alta(usuario);
                TempData["Success"] = "Usuario creado correctamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(Enum.GetValues<RolUsuario>(), usuario.Rol);
            return View(usuario);
        }

        // GET: Editar
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            ViewBag.Roles = new SelectList(Enum.GetValues<RolUsuario>(), usuario.Rol);
            return View(usuario);
        }

        // POST: Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(Usuario usuario, IFormFile? AvatarFile, bool RemoveAvatar = false, string? NuevaPassword = null)
        {
            var usuarioExistente = repo.ObtenerPorId(usuario.Id);
            if (usuarioExistente == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(NuevaPassword))
                usuarioExistente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NuevaPassword);

            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AvatarFile.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using var stream = new FileStream(path, FileMode.Create);
                AvatarFile.CopyTo(stream);
                usuarioExistente.Avatar = fileName;
            }
            else if (RemoveAvatar && !string.IsNullOrEmpty(usuarioExistente.Avatar))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", usuarioExistente.Avatar);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                usuarioExistente.Avatar = null;
            }

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Email = usuario.Email;

            repo.Modificacion(usuarioExistente);
            TempData["Success"] = "Usuario modificado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // POST: Baja lógica (solo admin mediante policy)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EliminarEmpleados")]
        public IActionResult Delete(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            if (usuario != null)
            {
                usuario.Activo = false;
                repo.Modificacion(usuario);
                TempData["Success"] = "Empleado dado de baja correctamente";
            }
            else
            {
                TempData["Error"] = "Empleado no encontrado";
            }

            return RedirectToAction(nameof(Index));
        }

        // PERFIL (accesible a cualquier usuario autenticado)
        public IActionResult Perfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var usuario = repo.ObtenerPorId(int.Parse(userId));
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Perfil(Usuario usuario, IFormFile? AvatarFile, bool RemoveAvatar = false, string? NuevaPassword = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var usuarioExistente = repo.ObtenerPorId(int.Parse(userId));
            if (usuarioExistente == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(NuevaPassword))
                usuarioExistente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NuevaPassword);

            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AvatarFile.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using var stream = new FileStream(path, FileMode.Create);
                AvatarFile.CopyTo(stream);
                usuarioExistente.Avatar = fileName;
            }
            else if (RemoveAvatar && !string.IsNullOrEmpty(usuarioExistente.Avatar))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", usuarioExistente.Avatar);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                usuarioExistente.Avatar = null;
            }

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Email = usuario.Email;

            repo.Modificacion(usuarioExistente);
            TempData["Success"] = "Perfil actualizado correctamente";
            return RedirectToAction("Perfil");
        }

        // POST: Eliminar físicamente (solo admin mediante policy)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EliminarEmpleados")]
        public IActionResult Eliminar(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            if (usuario != null)
            {
                if (!string.IsNullOrEmpty(usuario.Avatar))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", usuario.Avatar);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }

                repo.Eliminar(id);
                TempData["Error"] = "Empleado eliminado correctamente";
            }
            else
            {
                TempData["Error"] = "Empleado no encontrado";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
