using BLL.Services.Interfaces;
using BLL.Utilities.Interfaces;
using DAL.DTOs.ProviderDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class ProvidersController : Controller
    {
        private readonly IProviderService _providerService;
        private readonly ILogger<ProvidersController> _logger;
        private readonly IImageUploader _imageUploader;

        public ProvidersController(IProviderService providerService, ILogger<ProvidersController> logger, IImageUploader imageUploader)
        {
            _providerService = providerService;
            _logger = logger;
            _imageUploader = imageUploader;
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
            return View(new ProviderCreateDTO());
        }

        // POST: Providers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProviderCreateDTO providerDto)
        {
            if (ModelState.IsValid)
            {
                if (providerDto.ProviderImageFile != null && providerDto.ProviderImageFile.Length > 0)
                {
                    var uploadResult = await _imageUploader.UploadImageAsync(providerDto.ProviderImageFile);
                    if (uploadResult != null)
                    {
                        providerDto.ProviderImageUrl = uploadResult.Url;
                    }
                    else
                    {
                        ModelState.AddModelError("ProviderImageFile", "Image upload failed. Please try again.");
                        return View(providerDto);
                    }
                }

                try
                {
                    await _providerService.CreateProviderAsync(providerDto);
                    TempData["SuccessMessage"] = "Provider created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating provider.");
                    ModelState.AddModelError("", "Could not create the provider. Please try again.");
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

            return View(providerDto);
        }

        // POST: Providers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProviderEditDTO providerDto)
        {
            if (id != providerDto.ProviderId) return NotFound();

            if (ModelState.IsValid)
            {
                if (providerDto.ProviderImageFile != null && providerDto.ProviderImageFile.Length > 0)
                {
                    var uploadResult = await _imageUploader.UploadImageAsync(providerDto.ProviderImageFile);
                    if (uploadResult != null)
                    {
                        providerDto.ProviderImageUrl = uploadResult.Url;
                    }
                    else
                    {
                        ModelState.AddModelError("ProviderImageFile", "New image upload failed. Please try again.");
                        return View(providerDto);
                    }
                }

                try
                {
                    await _providerService.UpdateProviderAsync(id, providerDto);
                    TempData["SuccessMessage"] = "Provider updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating provider with ID {ProviderId}", id);
                    ModelState.AddModelError("", "Could not update the provider.");
                    return View(providerDto);
                }
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