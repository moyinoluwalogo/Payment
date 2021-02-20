using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Models
{
    public class PaymentState
    {
        public int Id { get; set; }
        public int PaymentDetailId { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt{ get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
