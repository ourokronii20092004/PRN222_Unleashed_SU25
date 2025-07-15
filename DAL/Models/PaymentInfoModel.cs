using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class PaymentInfoModel
    {
        public Guid OrderId { get; set; }
        public decimal? Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
    }
}
