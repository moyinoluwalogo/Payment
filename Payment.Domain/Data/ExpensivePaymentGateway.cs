using Microsoft.Extensions.Configuration;
using Payment.Domain.Dtos;
using Payment.Domain.Interfaces.PaymentProviders;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Data
{
    public class ExpensivePaymentGateway : IPaymentGateway
    {
        private PaymentDbContext _paymentDbContext;
        private readonly IConfiguration _configuration;
        public ExpensivePaymentGateway(PaymentDbContext paymentDbContext, IConfiguration configuration)
        {
            _paymentDbContext = paymentDbContext;
            _configuration = configuration;
        }

        public bool IsAvailable { get; set; }

        public void ProcessPayment(PaymentDetail paymentDetail)
        {
            var amount = paymentDetail.Amount;
            var LowerExpensiveAmountString = _configuration["LowerExpensiveAmount"];
            int LowerExpensiveAmount;
            int.TryParse(LowerExpensiveAmountString, out LowerExpensiveAmount);

            var PremiumAmountString = _configuration["PremiumAmount"];
            int PremiumAmount;
            int.TryParse(PremiumAmountString, out PremiumAmount);

            if (amount >= LowerExpensiveAmount && amount <= PremiumAmount)
            {
                PaymentState paymentState = _paymentDbContext.PaymentStates.FirstOrDefault(p => p.PaymentDetailId == paymentDetail.Id); ;
                if (IsAvailable)
                {
                    CheapPaymentGateway cheapPaymentGateway = new CheapPaymentGateway(_paymentDbContext, _configuration);
                    cheapPaymentGateway.ProcessPayment(paymentDetail);                    
                }
                else
                {
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
                }
            }

            if (amount > PremiumAmount)
            {
                PaymentState paymentState = _paymentDbContext.PaymentStates.FirstOrDefault(p => p.PaymentDetailId == paymentDetail.Id); ;
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
            }
        }
    }
}
