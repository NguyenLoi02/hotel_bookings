using hotel_bookings.Helpers;
using hotel_bookings.Models.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Internal;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace hotel_bookings.Models.Data
{
    public class VnPayService : IVnPayService
    {
        private readonly NameValueCollection _config;

        public VnPayService()
        {
            _config = ConfigurationManager.AppSettings;
        }

        public string CreatePaymentUrl(VnPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["Version"]); //Phiên bản cũ là 2.0.0, 2.0.1 thay đổi sang 2.1.0
            vnpay.AddRequestData("vnp_Command", _config["Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            //Số tiền thanh toán. Số tiền không
            //mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ.Để gửi số tiền thanh toán là 100,000 VND
            //(một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần(khử phần thập phân), sau đó gửi sang VNPAY
            //là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", _config["Locale"]);

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _config["PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);
            //Mã tham chiếu của giao dịch tại hệ
            //thống của merchant.Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.Không được
            //    trùng lặp trong ngày
            var paymentUrl = vnpay.CreateRequestUrl(_config["BaseUrl"], _config["HashSecret"]);
            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentExecute(NameValueCollection collection)
        {
            var vnpay = new VnPayLibrary();

            foreach (string key in collection.AllKeys)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, collection[key]);
                }
            }

            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collection["vnp_SecureHash"];
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["HashSecret"]);

            //if (!checkSignature)
            //{
            //    return new VnPaymentResponseModel
            //    {
            //        Success = false,
            //    };
            //}

            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode,
            };
        }

    }
}