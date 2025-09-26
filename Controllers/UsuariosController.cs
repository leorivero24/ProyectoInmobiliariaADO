using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliariaADO.Data;
using ProyectoInmobiliariaADO.Models;
using System.Security.Claims;

namespace ProyectoInmobiliariaADO.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly RepositorioUsuario repo = new RepositorioUsuario();

        // GET: Usuarios
        public IActionResult Index()
        {
            if (!User.IsInRole("Administrador"))
            {
                // Empleados no pueden ver la lista completa, los redirige a su perfil
                return RedirectToAction("Perfil");
            }

            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Detalles
        public IActionResult Details(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // GET: Crear
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(Enum.GetValues<RolUsuario>());
            return View();
        }

        // POST: Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public IActionResult Edit(Usuario usuario, IFormFile? AvatarFile, bool RemoveAvatar = false, string? NuevaPassword = null)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = repo.ObtenerPorId(usuario.Id);
                if (usuarioExistente == null) return NotFound();

                // Contraseña
                if (!string.IsNullOrWhiteSpace(NuevaPassword))
                    usuarioExistente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NuevaPassword);

                // Avatar
                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AvatarFile.FileName)}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                    using var stream = new FileStream(path, FileMode.Create);
                    AvatarFile.CopyTo(stream);
                    usuarioExistente.Avatar = fileName;
                }
                else if (RemoveAvatar)
                {
                    // Elimina la ruta del avatar
                    if (!string.IsNullOrEmpty(usuarioExistente.Avatar))
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", usuarioExistente.Avatar);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    usuarioExistente.Avatar = null;
                }

                // Actualizar otros campos
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Rol = usuarioExistente.Rol; // No modificar rol desde edición normal
                usuarioExistente.FechaCreacion = usuarioExistente.FechaCreacion;
                usuarioExistente.FechaUltimoLogin = usuarioExistente.FechaUltimoLogin;

                repo.Modificacion(usuarioExistente);
                TempData["Success"] = "Usuario modificado correctamente";

                // Redirige al Index si es admin, si no vuelve al Perfil
                return User.IsInRole("Administrador") ? RedirectToAction(nameof(Index)) : RedirectToAction("Perfil");
            }

            ViewBag.Roles = new SelectList(Enum.GetValues<RolUsuario>(), usuario.Rol);
            return View(usuario);
        }

        // POST: Eliminar (baja lógica)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            if (usuario != null)
            {
                usuario.Activo = false;
                repo.Modificacion(usuario);
                TempData["Error"] = "Usuario dado de baja correctamente";
            }
            else
            {
                TempData["Error"] = "Usuario no encontrado";
            }

            return RedirectToAction(nameof(Index));
        }

        // ===== PERFIL =====
        // GET: Perfil
        public IActionResult Perfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var usuario = repo.ObtenerPorId(int.Parse(userId));
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // POST: Perfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Perfil(Usuario usuario, IFormFile? AvatarFile, bool RemoveAvatar = false, string? NuevaPassword = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var usuarioExistente = repo.ObtenerPorId(int.Parse(userId));
            if (usuarioExistente == null) return NotFound();

            // Contraseña
            if (!string.IsNullOrWhiteSpace(NuevaPassword))
                usuarioExistente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NuevaPassword);

            // Avatar
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AvatarFile.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using var stream = new FileStream(path, FileMode.Create);
                AvatarFile.CopyTo(stream);
                usuarioExistente.Avatar = fileName;
            }
            else if (RemoveAvatar)
            {
                if (!string.IsNullOrEmpty(usuarioExistente.Avatar))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", usuarioExistente.Avatar);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath); // elimina el archivo físico
                    }
                }
                usuarioExistente.Avatar = null;
            }

            // Actualizar solo datos personales
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Email = usuario.Email;

            repo.Modificacion(usuarioExistente);
            TempData["Success"] = "Perfil actualizado correctamente";

            // Después de guardar, vuelve al perfil
            return RedirectToAction("Perfil");
        }

        // POST: Eliminar físicamente un usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            var usuario = repo.ObtenerPorId(id);
            if (usuario != null)
            {
                // Eliminar avatar físico si existe
                if (!string.IsNullOrEmpty(usuario.Avatar))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", usuario.Avatar);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                repo.Eliminar(id); // Llama al método físico en el repositorio
                TempData["Error"] = "Usuario eliminado correctamente";
            }
            else
            {
                TempData["Error"] = "Usuario no encontrado";
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
