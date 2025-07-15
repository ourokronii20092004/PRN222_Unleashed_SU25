using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class VnpayTransactionResponse
    {
        public bool Success { get; set; }
        public bool IsValidSignature { get; set; }
        public Guid? OrderId { get; set; }
        public string TransactionId { get; set; }
        public string VnPayResponseCode { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string Token { get; set; }
    }
}
