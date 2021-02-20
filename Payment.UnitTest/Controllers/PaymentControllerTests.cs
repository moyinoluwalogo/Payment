using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Payment.API.Controllers;
using Payment.Domain.Data;
using Payment.Domain.Dtos;
using Payment.Domain.Enumerations;
using Payment.Domain.Interfaces;
using Payment.Domain.Models;
using Payment.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Payment.UnitTest.Controllers
{
    public class PaymentControllerTests
    {
        private readonly Mock<IGatewayFactory> _gatewayFactory;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<ILogger<PaymentController>> _logger;
        private readonly PaymentController _paymentController;
        public PaymentControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _gatewayFactory = new Mock<IGatewayFactory>();
            _configuration = new Mock<IConfiguration>();
            _logger = new Mock<ILogger<PaymentController>>();
            _paymentController = new PaymentController(_gatewayFactory.Object, _mapper.Object, _configuration.Object, _logger.Object);
        }

        [Fact]
        public void ProcessPayment_ActionExecutes_ReturnActionResultOfTypeResponse()
        {
            PaymentDetailRequestDto paymentDetail = new PaymentDetailRequestDto
            {
                Amount = 19.0M,
                CardHolder = "CardHolder Name",
                CreditCardNumber = "21232123407",
                ExpirationDate = DateTime.Today.AddDays(2.0),
                RequestId = Guid.NewGuid().ToString(),
                SecurityCode = "234",                
            };

            var response = _paymentController.ProcessPayment(paymentDetail);
            Assert.IsType<ActionResult<Response>>(response);
        }


    }
}
