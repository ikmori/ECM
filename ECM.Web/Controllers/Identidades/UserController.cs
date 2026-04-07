using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Domain.Entities.Identidades;
using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Identidades;

public class UserController : Controller
{
    // GET
    private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: User/Index
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllAsync();
            
            // Si la lista está vacía o hay un error, pasamos el mensaje a la vista
            if (!result.Success)
            {
                ViewBag.InfoMessage = result.Message;
            }

            return View(result.Data);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // GET: User/Create
        // Muestra el formulario vacío
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid) return View(user);

            var result = await _userService.CreateAsync(user);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            // Muestra el error (ej. email duplicado) en el mismo formulario
            TempData["Error"] = result.Message;
            return View(user);
        }

        // GET: User/Edit/5
        // Busca al usuario y llena el formulario
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                TempData["Error"] = "El ID del usuario no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) return View(user);

            var result = await _userService.UpdateAsync(user);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Message;
            return View(user);
        }

        // GET: User/Delete/5
        // Vista opcional para confirmar antes de desactivar
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // POST: User/Delete/5
        // Realiza el borrado lógico en la base de datos
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }
}