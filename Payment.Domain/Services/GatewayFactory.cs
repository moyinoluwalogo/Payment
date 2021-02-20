using Microsoft.Extensions.Configuration;
using Payment.Domain.Data;
using Payment.Domain.Enumerations;
using Payment.Domain.Extensions;
using Payment.Domain.Interfaces;
using Payment.Domain.Interfaces.PaymentProviders;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Services
{
    public class GatewayFactory : IGatewayFactory
    {
        private readonly PaymentDbContext _paymentDbContext;
        private readonly IConfiguration _configuration;
        public GatewayFactory(PaymentDbContext paymentDbContext, IConfiguration configuration)
        {
            _paymentDbContext = paymentDbContext;
            _configuration = configuration;
        }

        public Response Process(PaymentDetail paymentDetail)
        {
            var beforeMaskPaymentDetail = new PaymentDetail { 
                Amount = paymentDetail.Amount,
                CardHolder = paymentDetail.CardHolder,
                CreditCardNumber = paymentDetail.CreditCardNumber,
                ExpirationDate = paymentDetail.ExpirationDate,
                SecurityCode = paymentDetail.SecurityCode,
                RequestId = paymentDetail.RequestId
            };

            var maskedCardNumber = paymentDetail;
            maskedCardNumber.CreditCardNumber = HideNumber(paymentDetail.CreditCardNumber);
            maskedCardNumber.SecurityCode = HideNumber(paymentDetail.SecurityCode);
            maskedCardNumber.CreatedAt = DateTime.Now;
            _paymentDbContext.PaymentDetails.Add(maskedCardNumber);
            _paymentDbContext.SaveChanges();

            beforeMaskPaymentDetail.Id = maskedCardNumber.Id;
            paymentDetail = beforeMaskPaymentDetail;

            PaymentState paymentState1 = new PaymentState
            {
                CreatedAt = DateTime.Now,
                PaymentDetailId = paymentDetail.Id,
                Status = _configuration["Pending"]
            };
            _paymentDbContext.PaymentStates.Add(paymentState1);
            _paymentDbContext.SaveChanges();

            new CheapPaymentGateway(_paymentDbContext, _configuration).ProcessPayment(paymentDetail);
            new ExpensivePaymentGateway(_paymentDbContext, _configuration).ProcessPayment(paymentDetail);

            var paymentState = _paymentDbContext.PaymentStates.FirstOrDefault(p => p.PaymentDetailId == paymentDetail.Id);
            switch (paymentState.Status)
            {
                case "Y":
                    return new Response
                    {
                        Code = ResponseEnum.ApprovedOrCompletedSuccesfully.ResponseCode(),
                        Description = ResponseEnum.ApprovedOrCompletedSuccesfully.DisplayName()
                    };
                case "P":
                    return new Response
                    {
                        Code = ResponseEnum.PendingStatus.ResponseCode(),
                        Description = ResponseEnum.PendingStatus.DisplayName()
                    };
                default:
                    return new Response
                    {
                        Code = ResponseEnum.Failure.ResponseCode(),
                        Description = ResponseEnum.Failure.DisplayName()
                    };
            }

        }

        private string HideNumber(string number)
        {
            string hiddenString;
            if (number.Length < 4) {
                hiddenString = number.Substring(number.Length - 1).PadLeft(number.Length, '*');
                return hiddenString;
            }
            hiddenString = number.Substring(number.Length - 4).PadLeft(number.Length, '*');
            return hiddenString;
        }

    }
}
