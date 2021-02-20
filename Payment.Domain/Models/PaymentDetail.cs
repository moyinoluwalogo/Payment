using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Models
{
    public class PaymentDetail
    {
        public int Id { get; set; }
        [Required]
        public string RequestId { get; set; }
        [Required]
        public string CreditCardNumber { get; set; }
        [Required]
        public string CardHolder { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [ScaffoldColumn(true)]
        public string SecurityCode { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}
