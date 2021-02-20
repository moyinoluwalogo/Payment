using Microsoft.Extensions.Configuration;
using Payment.Domain.Data;
using Payment.Domain.Interfaces.PaymentProviders;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Data
{
    public class PremiumPaymentService : IPaymentGateway
    {
        private PaymentDbContext _paymentDbContext;
        private readonly IConfiguration _configuration;
        public PremiumPaymentService(PaymentDbContext paymentDbContext, IConfiguration configuration)
        {
            _paymentDbContext = paymentDbContext;
            _configuration = configuration;
        }

        public void ProcessPayment(PaymentDetail paymentDetail)
        {
            var PremiumAmountString = _configuration["PremiumAmount"];
            int PremiumAmount;
            int.TryParse(PremiumAmountString, out PremiumAmount);
            if (paymentDetail.Amount > PremiumAmount)
            {
                var premiumRetriesString = _configuration["PremiumRetries"];
                int premiumRetries;
                int.TryParse(premiumRetriesString, out premiumRetries);

                var waitTimeString = _configuration["WaitTime"];
                int waitTime;
                int.TryParse(waitTimeString, out waitTime);

                for (int i=0; i < premiumRetries; i++)
                {                   
                    PaymentState paymentState = _paymentDbContext.PaymentStates.FirstOrDefault(p => p.PaymentDetailId == paymentDetail.Id);
                    if (paymentState.Status == _configuration["Success"]) break;
                    ExpensivePaymentGateway expensivePaymentGateway = new ExpensivePaymentGateway(_paymentDbContext, _configuration);
                    expensivePaymentGateway.ProcessPayment(paymentDetail);
                    System.Threading.Thread.Sleep(waitTime * 1000);
                }
                
            }
           
        }
    }
}
