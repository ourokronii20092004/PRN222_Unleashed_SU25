using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Unleashed_RP.Pages.Payment
{
    public class CheckoutModel : PageModel
    {
        private readonly IVnpayService _vnpayService;
        private readonly ILogger<CheckoutModel> _logger;

        public CheckoutModel(IVnpayService vnpayService, ILogger<CheckoutModel> logger)
        {
            _vnpayService = vnpayService;
            _logger = logger;
        }

        public IActionResult OnPost()
        {
            var paymentModel = new PaymentInfoModel
            {
                OrderId = DateTime.Now.Ticks,
                Amount = 10000,
                Name = "Customer Name",
                OrderDescription = "Payment for order 123"
            };

            var paymentUrl = _vnpayService.CreatePaymentUrl(paymentModel);

            _logger.LogInformation("Redirecting to VNPAY URL: {Url}", paymentUrl);

            return Redirect(paymentUrl);
        }
    }
}
