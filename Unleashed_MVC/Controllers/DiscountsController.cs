using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.DiscountDTOs;
using DAL.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class DiscountsController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly ILogger<DiscountsController> _logger;
        private const int FIXED_AMOUNT_TYPE_ID = 1;
        private const int PERCENTAGE_TYPE_ID = 2;
        private const int ACTIVE_STATUS_ID = 1;
        private const int INACTIVE_STATUS_ID = 3;

        public DiscountsController(IDiscountService discountService, ILogger<DiscountsController> logger)
        {
            _discountService = discountService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var discounts = await _discountService.GetAllDiscountsAsync();
            return View(discounts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);
            if (discount == null) return NotFound();
            return View(discount);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdownData();
            return View(new DiscountCreateDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountCreateDTO discountDto)
        {

            if (discountDto.DiscountStartDate >= discountDto.DiscountEndDate)
            {
                ModelState.AddModelError("DiscountEndDate", "End date must be after start date.");
            }

            if (discountDto.DiscountTypeId == PERCENTAGE_TYPE_ID)
            {
                if (discountDto.DiscountValue < 1 || discountDto.DiscountValue > 100)
                {
                    ModelState.AddModelError(nameof(discountDto.DiscountValue), "For PERCENTAGE type, the value must be between 1 and 100.");
                }
            }
            else if (discountDto.DiscountTypeId == FIXED_AMOUNT_TYPE_ID)
            {
                if (discountDto.DiscountValue < 10000 || discountDto.DiscountValue > 99999999)
                {
                    ModelState.AddModelError(nameof(discountDto.DiscountValue), "For FIXED AMOUNT type, the value must be at least 10,000 and lower than 99999999");
                }
            }

            if (discountDto.DiscountStatusId == ACTIVE_STATUS_ID && discountDto.DiscountStartDate > DateTimeOffset.UtcNow)
            {
                ModelState.AddModelError(nameof(discountDto.DiscountStatusId), "Cannot set status to Active before the start date.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _discountService.CreateDiscountAsync(discountDto);
                    TempData["SuccessMessage"] = "Discount created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("DiscountCode", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating discount.");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }
            await LoadDropdownData();
            return View(discountDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var discountDto = await _discountService.GetDiscountForUpdateAsync(id);
            if (discountDto == null) return NotFound();

            await LoadDropdownData(discountDto.DiscountStatusId, discountDto.DiscountTypeId);
            return View(discountDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DiscountUpdateDTO discountDto)
        {
            if (id != discountDto.DiscountId) return NotFound();

            if (discountDto.DiscountStartDate >= discountDto.DiscountEndDate)
            {
                ModelState.AddModelError("DiscountEndDate", "End date must be after start date.");
            }

            if (discountDto.DiscountTypeId == PERCENTAGE_TYPE_ID)
            {
                if (discountDto.DiscountValue < 1 || discountDto.DiscountValue > 100)
                {
                    ModelState.AddModelError(nameof(discountDto.DiscountValue), "For PERCENTAGE type, the value must be between 1 and 100.");
                }
            }
            else if (discountDto.DiscountTypeId == FIXED_AMOUNT_TYPE_ID)
            {
                if (discountDto.DiscountValue < 10000)
                {
                    ModelState.AddModelError(nameof(discountDto.DiscountValue), "For FIXED AMOUNT type, the value must be at least 10,000.");
                }
            }

            if (discountDto.DiscountStatusId == ACTIVE_STATUS_ID && discountDto.DiscountStartDate > DateTimeOffset.UtcNow)
            {
                ModelState.AddModelError(nameof(discountDto.DiscountStatusId), "Cannot set status to Active before the start date.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _discountService.UpdateDiscountAsync(id, discountDto);
                    TempData["SuccessMessage"] = "Discount updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("DiscountCode", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating discount {DiscountId}", id);
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            await LoadDropdownData(discountDto.DiscountStatusId, discountDto.DiscountTypeId);
            return View(discountDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);
            if (discount == null) return NotFound();
            return View(discount);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _discountService.DeleteDiscountAsync(id);
                TempData["SuccessMessage"] = "Discount deleted successfully.";
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting discount {DiscountId}", id);
                TempData["ErrorMessage"] = "Error deleting discount. It might be in use.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownData(int? selectedStatusId = null, int? selectedTypeId = null)
        {
            ViewBag.DiscountStatusId = new SelectList(await _discountService.GetDiscountStatusesAsync(), "Value", "Text", selectedStatusId);
            ViewBag.DiscountTypeId = new SelectList(await _discountService.GetDiscountTypesAsync(), "Value", "Text", selectedTypeId);
        }
    }
}