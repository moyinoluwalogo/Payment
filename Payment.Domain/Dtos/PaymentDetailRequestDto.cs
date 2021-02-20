using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Dtos
{
    public class PaymentDetailRequestDto
    {
        [Required(ErrorMessage = "RequestId is required")]
        public string RequestId { get; set; }

        [Required(ErrorMessage = "CreditCardNumber is required")]
        public string CreditCardNumber { get; set; }
        [Required(ErrorMessage = "CardHolder is required")]
        public string CardHolder { get; set; }
        [Required(ErrorMessage = "ExpirationDate is required")]
        public DateTime ExpirationDate { get; set; }
        [ScaffoldColumn(true)]
        public string SecurityCode { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
    }
}
