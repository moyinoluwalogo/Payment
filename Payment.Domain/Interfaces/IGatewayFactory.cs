using Payment.Domain.Data;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Interfaces
{
    public interface IGatewayFactory
    {
        Response Process(PaymentDetail paymentDetail);
    }
}
