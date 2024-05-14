using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;



namespace hotel_bookings.Models.Service
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(NameValueCollection collecttion);
    }
}
