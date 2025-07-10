using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class VnpayRespond
    {
        public bool IsValidSignature { get; set; }
        public string TxnRef { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionStatus { get; set; }
        public long Amount { get; set; }
        public string TransactionNo { get; set; }
        public string BankCode { get; set; }
    }
}
