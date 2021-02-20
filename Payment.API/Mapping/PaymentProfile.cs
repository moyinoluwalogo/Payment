using AutoMapper;
using Payment.Domain.Dtos;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Mapping
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentDetailRequestDto, PaymentDetail>();
        }
        
    }
}
