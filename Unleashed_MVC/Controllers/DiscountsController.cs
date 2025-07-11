using DAL.Models; // Thêm using này
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering; // Thêm using này
using DAL.Data;
using BLL.Services.Interfaces; // Thêm using này để truy cập DbContext

namespace Unleashed_MVC.Controllers
{
    [Filter.Filter(RequiredRoles = new[] { "ADMIN", "STAFF" })]
    public class DiscountsController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly UnleashedContext _context; // Thêm DbContext để lấy list cho dropdown

        // Sửa constructor
        public DiscountsController(IDiscountService discountService, UnleashedContext context)
        {
            _discountService = discountService;
            _context = context;
        }

        // GET: Discounts
        public async Task<IActionResult> Index()
        {
            var discounts = await _discountService.GetAllDiscountsAsync();
            return View(discounts);
        }

        // --- CÁC ACTION MỚI ---

        // GET: Discounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var discount = await _discountService.GetDiscountByIdAsync(id.Value);
            if (discount == null) return NotFound();
            return View(discount);
        }

        // GET: Discounts/Create
        public IActionResult Create()
        {
            ViewData["DiscountStatusId"] = new SelectList(_context.DiscountStatuses, "DiscountStatusId", "DiscountStatusName");
            ViewData["DiscountTypeId"] = new SelectList(_context.DiscountTypes, "DiscountTypeId", "DiscountTypeName");
            return View();
        }

        // POST: Discounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscountStatusId,DiscountTypeId,DiscountCode,DiscountValue,DiscountDescription,DiscountMinimumOrderValue,DiscountMaximumValue,DiscountUsageLimit,DiscountStartDate,DiscountEndDate,DiscountUsageCount")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                await _discountService.CreateDiscountAsync(discount);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountStatusId"] = new SelectList(_context.DiscountStatuses, "DiscountStatusId", "DiscountStatusName", discount.DiscountStatusId);
            ViewData["DiscountTypeId"] = new SelectList(_context.DiscountTypes, "DiscountTypeId", "DiscountTypeName", discount.DiscountTypeId);
            return View(discount);
        }

        // GET: Discounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var discount = await _discountService.GetDiscountByIdAsync(id.Value);
            if (discount == null) return NotFound();

            ViewData["DiscountStatusId"] = new SelectList(_context.DiscountStatuses, "DiscountStatusId", "DiscountStatusName", discount.DiscountStatusId);
            ViewData["DiscountTypeId"] = new SelectList(_context.DiscountTypes, "DiscountTypeId", "DiscountTypeName", discount.DiscountTypeId);
            return View(discount);
        }

        // POST: Discounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscountId,DiscountStatusId,DiscountTypeId,DiscountCode,DiscountValue,DiscountDescription,DiscountMinimumOrderValue,DiscountMaximumValue,DiscountUsageLimit,DiscountStartDate,DiscountEndDate,DiscountCreatedAt,DiscountUsageCount")] Discount discount)
        {
            if (id != discount.DiscountId)
            {
                return NotFound();
            }

            ModelState.Remove("DiscountType");
            ModelState.Remove("DiscountStatus");

            // --- LOGIC NGHIỆP VỤ ---
            // Giả sử ID của "FIXED AMOUNT" là 1, và của "ACTIVE" / "INACTIVE" là 1 và 2
            // Bạn nên lấy các ID này từ database thay vì hard-code
            const int PERCENTAGE_TYPE_ID = 2; // THAY ĐỔI NẾU CẦN
            const int FIXED_AMOUNT_TYPE_ID = 1;
            const int ACTIVE_STATUS_ID = 1;     // THAY ĐỔI NẾU CẦN
            const int INACTIVE_STATUS_ID = 3;   // THAY ĐỔI NẾU CẦN

            // Kiểm tra logic cho FIXED AMOUNT
            if (discount.DiscountTypeId == PERCENTAGE_TYPE_ID)
            {
                if (discount.DiscountValue < 1 || discount.DiscountValue > 100)
                {
                    ModelState.AddModelError("DiscountValue", "For PERCENTAGE, value must be between 1 and 100.");
                }
            }
            if (discount.DiscountTypeId == FIXED_AMOUNT_TYPE_ID)
            {
                if (discount.DiscountValue > discount.DiscountMaximumValue)
                {
                    ModelState.AddModelError("DiscountValue", "DiscountValue can not higher DiscountMaximumValue");
                }
            }
            // Tự động cập nhật status dựa trên ngày bắt đầu
            if (discount.DiscountStartDate <= DateTime.Now && discount.DiscountEndDate >= DateTime.Now)
            {
                discount.DiscountStatusId = ACTIVE_STATUS_ID;
            }
            else if (discount.DiscountStartDate > DateTime.Now || discount.DiscountEndDate < DateTime.Now)
            {
                discount.DiscountStatusId = INACTIVE_STATUS_ID;
            }
            else if (discount.DiscountStartDate > discount.DiscountEndDate)
            {
                ModelState.AddModelError("DiscountEndDate", "End time not before start date.");
            }
            else
            {
                discount.DiscountStatusId = ACTIVE_STATUS_ID;
            }

            // --- KẾT THÚC LOGIC NGHIỆP VỤ ---

            if (ModelState.IsValid)
            {
                var result = await _discountService.UpdateDiscountAsync(discount);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }

            // Nếu model state không hợp lệ, tải lại dropdown list và trả về view
            ViewData["DiscountStatusId"] = new SelectList(_context.DiscountStatuses, "DiscountStatusId", "DiscountStatusName", discount.DiscountStatusId);
            ViewData["DiscountTypeId"] = new SelectList(_context.DiscountTypes, "DiscountTypeId", "DiscountTypeName", discount.DiscountTypeId);
            return View(discount);
        }

        // GET: Discounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var discount = await _discountService.GetDiscountByIdAsync(id.Value);
            if (discount == null) return NotFound();
            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _discountService.DeleteDiscountAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}