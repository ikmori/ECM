using Microsoft.AspNetCore.Mvc;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Domain.Entities.Identidades;
using System.Threading.Tasks;

namespace ECM.Web.Controllers.Identidades
{
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // GET: Address/Index
        public async Task<IActionResult> Index()
        {
            var result = await _addressService.GetAllAsync();
            
            // Si la lista está vacía o el servicio devuelve una alerta, lo pasamos a la vista
            if (!result.Success)
            {
                ViewBag.InfoMessage = result.Message;
            }

            return View(result.Data);
        }

        // GET: Address/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Address/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            // El formulario debe enviar un UserId válido, de lo contrario esto fallará
            if (!ModelState.IsValid) return View(address);

            var result = await _addressService.CreateAsync(address);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Message;
            return View(address);
        }

        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _addressService.GetByIdAsync(id);
            
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // POST: Address/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Address address)
        {
            if (id != address.Id)
            {
                TempData["Error"] = "El ID de la dirección no coincide con el registro.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) return View(address);

            var result = await _addressService.UpdateAsync(address);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Message;
            return View(address);
        }

        // POST: Address/Delete/5
        // Nota: Solo usamos POST para eliminar por razones de seguridad
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _addressService.DeleteAsync(id);

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
}