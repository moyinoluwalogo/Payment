using Microsoft.Extensions.Configuration;
using Payment.Domain.Dtos;
using Payment.Domain.Enumerations;
using Payment.Domain.Interfaces.PaymentProviders;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Data
{
    public class CheapPaymentGateway : IPaymentGateway
    {
        private readonly PaymentDbContext _paymentDbContext;
        private readonly IConfiguration _configuration;
        public CheapPaymentGateway(PaymentDbContext paymentDbContext, IConfiguration configuration)
        {
            _paymentDbContext = paymentDbContext;
            _configuration = configuration;
        }
        public void ProcessPayment(PaymentDetail paymentDetail)
        {
            var CheapAmountString = _configuration["CheapAmount"];
            int CheapAmount;
            int.TryParse(CheapAmountString, out CheapAmount);

            if (paymentDetail.Amount <= CheapAmount) {

                var paymentState = _paymentDbContext.PaymentStates.FirstOrDefault(p => p.PaymentDetailId == paymentDetail.Id);
                if (paymentState != null)
                {
                    paymentState.Status = _configuration["Success"];
                    paymentState.UpdatedAt = DateTime.Now;
                    _paymentDbContext.PaymentStates.Update(paymentState);
                    _paymentDbContext.SaveChanges();
                }
                else
                {
                    paymentState.Status = _configuration["Failure"];
                    paymentState.UpdatedAt = DateTime.Now;
                    _paymentDbContext.PaymentStates.Update(paymentState);
                    _paymentDbContext.SaveChanges();
                }

            };

        }

    }
}
