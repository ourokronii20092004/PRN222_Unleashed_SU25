using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IVnpayService
    {

        public VnpayRespond ProcessVnpayResponse(IQueryCollection query);

        string CreatePaymentUrl(PaymentInfoModel model);


    }
}
