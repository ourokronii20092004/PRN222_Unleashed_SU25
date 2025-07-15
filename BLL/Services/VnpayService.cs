using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using VNPAY_CS_ASPX;

namespace BLL.Services
{


    public class VnpayService : IVnpayService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<VnpayService> _logger;

        public VnpayService(IConfiguration config, IHttpContextAccessor contextAccessor, ILogger<VnpayService> logger)
        {
            _config = config;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public VnpayTransactionResponse GetPaymentResponse(IQueryCollection queryString)
        {
            var vnpay = new VnPayLibrary();
            var vnp_HashSecret = _config["Vnpay:HashSecret"];

            foreach (var (key, value) in queryString)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }
            string vnp_TxnRef_str = vnpay.GetResponseData("vnp_TxnRef");
            Guid? orderId = null;
            if (Guid.TryParse(vnp_TxnRef_str, out Guid parsedGuid))
            {
                orderId = parsedGuid;
            }
            else
            {
                _logger.LogError("VNPay response is missing or has an invalid 'vnp_TxnRef'. Query: {query}", queryString.ToString());
            }

            var vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (!checkSignature)
            {
                _logger.LogError("VNPay response has an invalid signature. Query: {query}", queryString.ToString());
            }

            return new VnpayTransactionResponse
            {
                Success = vnpay.GetResponseData("vnp_ResponseCode") == "00",
                OrderId = orderId,
                TransactionId = vnpay.GetResponseData("vnp_TransactionNo"),
                VnPayResponseCode = vnp_ResponseCode,
                IsValidSignature = checkSignature
            };
        }

        public string CreatePaymentUrl(PaymentInfoModel model, HttpContext context)
        {
            var pay = new VnPayLibrary();
            var urlCallBack = _config["Vnpay:ReturnUrl"];
            var tmnCode = _config["Vnpay:TmnCode"];
            var baseUrl = _config["Vnpay:BaseUrl"];
            var hashSecret = _config["Vnpay:HashSecret"];

            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", tmnCode);
            pay.AddRequestData("vnp_Amount", ((long)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", GetIpAddress());
            pay.AddRequestData("vnp_Locale", "vn");
            string unsafeOrderInfo = $"{model.Name} {model.OrderDescription}";
            string sanitizedOrderInfo = System.Text.RegularExpressions.Regex.Replace(unsafeOrderInfo, @"[^\w\s]", "");
            pay.AddRequestData("vnp_OrderInfo", sanitizedOrderInfo);
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", model.OrderId.ToString());

            string paymentUrl = pay.CreateRequestUrl(baseUrl, hashSecret);
            _logger.LogInformation("Created VNPAY URL for OrderId {OrderId}: {Url}", model.OrderId, paymentUrl);
            return paymentUrl;
        }


        private string GetIpAddress()
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null) return "127.0.0.1";
            var ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ipAddress)) ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            if (ipAddress == "::1") ipAddress = "127.0.0.1";
            return ipAddress ?? "127.0.0.1";
        }
    }
}