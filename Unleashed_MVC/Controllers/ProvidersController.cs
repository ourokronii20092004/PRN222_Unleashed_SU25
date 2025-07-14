using BLL.Services.Interfaces;
using DAL.DTOs.ProviderDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class ProvidersController : Controller
    {
        private readonly IProviderService _providerService;
        private readonly ILogger<ProvidersController> _logger;

        public ProvidersController(IProviderService providerService, ILogger<ProvidersController> logger)
        {
            _providerService = providerService;
            _logger = logger;
        }

        // GET: Providers
        public async Task<IActionResult> Index()
        {
            var providers = await _providerService.GetAllProvidersAsync();
            return View(providers);
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var provider = await _providerService.GetProviderByIdAsync(id.Value);
            if (provider == null) return NotFound();

            return View(provider);
        }

        // GET: Providers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProviderName,ProviderImageUrl,ProviderEmail,ProviderPhone,ProviderAddress")] ProviderCreateDTO providerDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _providerService.CreateProviderAsync(providerDto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating provider.");
                    ModelState.AddModelError("", "Không thể tạo nhà cung cấp. Vui lòng thử lại.");
                }
            }
            return View(providerDto);
        }

        // GET: Providers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var providerDto = await _providerService.GetProviderForEditAsync(id.Value);
            if (providerDto == null) return NotFound();
            Console.WriteLine(providerDto);

            return View(providerDto);
        }

        // POST: Providers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProviderId,ProviderName,ProviderImageUrl,ProviderEmail,ProviderPhone,ProviderAddress")] ProviderEditDTO providerDto)
        {
            Console.WriteLine(providerDto);
            if (id != providerDto.ProviderId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _providerService.UpdateProviderAsync(id, providerDto);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating provider with ID {ProviderId}", id);
                    ModelState.AddModelError("", "Không thể cập nhật nhà cung cấp. Vui lòng thử lại.");
                    return View(providerDto);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(providerDto);
        }

        // GET: Providers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var provider = await _providerService.GetProviderByIdAsync(id.Value);
            if (provider == null) return NotFound();

            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _providerService.DeleteProviderAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting provider with ID {ProviderId}", id);

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}