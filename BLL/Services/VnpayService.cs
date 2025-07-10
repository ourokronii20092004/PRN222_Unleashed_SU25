using DAL.Models;
using VNPAY_CS_ASPX;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Logging;

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

        public VnpayRespond ProcessVnpayResponse(IQueryCollection queryString)
        {
            var vnpay = new VnPayLibrary();
            string vnp_HashSecret = _config["Vnpay:HashSecret"];

            foreach (var (key, value) in queryString)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount"));
            string vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            return new VnpayRespond
            {
                Amount = vnp_Amount,
                TxnRef = vnp_TxnRef,
                ResponseCode = vnp_ResponseCode,
                TransactionStatus = vnp_TransactionStatus,
                IsValidSignature = checkSignature,
                TransactionNo = vnpay.GetResponseData("vnp_TransactionNo"),
                BankCode = vnpay.GetResponseData("vnp_BankCode")
            };
        }

        private string GetIpAddress()
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null)
            {
                return "127.0.0.1"; // Fallback
            }

            // Check for the X-Forwarded-For header, which is common for proxies
            var ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            // If the header is not present, get the remote IP address from the connection
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            }

            // Handle the case of IPv6 loopback address (::1) and map it to IPv4 loopback
            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }

            return ipAddress ?? "127.0.0.1"; // Return a fallback if still null
        }



        public string CreatePaymentUrl(PaymentInfoModel model)
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

            _logger.LogDebug(paymentUrl);

            return paymentUrl;
        }

        






    }
}