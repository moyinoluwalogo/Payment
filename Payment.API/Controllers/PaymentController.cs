using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Payment.Domain.Data;
using Payment.Domain.Dtos;
using Payment.Domain.Enumerations;
using Payment.Domain.Interfaces;
using Payment.Domain.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IGatewayFactory _gatewayFactory;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentController> _logger = null;
        public PaymentController(IGatewayFactory gatewayFactory, IMapper mapper, IConfiguration configuration, ILogger<PaymentController> logger)
        {
            _mapper = mapper;
            _gatewayFactory = gatewayFactory;
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost]
        [Route("ProcessPayment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Response> ProcessPayment([FromBody] PaymentDetailRequestDto paymentDetailRequestDto)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var minimumAmountString = _configuration["MinimumAmount"];
                decimal minimumAmount;
                decimal.TryParse(minimumAmountString, out minimumAmount);

                if (paymentDetailRequestDto.Amount <= minimumAmount) { 
                    return BadRequest(
                        new Response
                        {
                            Code = ResponseEnum.BadRequest.ResponseCode(),
                            Description = ResponseEnum.BadRequest.DisplayName()
                        });
                }

                if (paymentDetailRequestDto.ExpirationDate < DateTime.Now) return BadRequest("ExpiryDate has passed");

                var securityCodeLengthString = _configuration["SecurityCodeLength"];
                int securityCodeLength;
                int.TryParse(securityCodeLengthString, out securityCodeLength);

                if (!string.IsNullOrWhiteSpace(paymentDetailRequestDto.SecurityCode) && paymentDetailRequestDto.SecurityCode.Length != securityCodeLength)
                    return BadRequest("Security Code is of three characters");

                var ccardLengthString = _configuration["CcardLength"];
                int ccardLength;
                int.TryParse(ccardLengthString, out ccardLength);

                var minCcardLengthString = _configuration["MinCcardLength"];
                int mCcardLength;
                int.TryParse(minCcardLengthString, out mCcardLength);

                if (paymentDetailRequestDto.CreditCardNumber.Length > ccardLength || paymentDetailRequestDto.CreditCardNumber.Length < mCcardLength)
                    return BadRequest("Invalid card length");

                var paymentDetail = _mapper.Map<PaymentDetailRequestDto, PaymentDetail>(paymentDetailRequestDto);
                var response =  _gatewayFactory.Process(paymentDetail);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, new Response
                {
                    Code = ResponseEnum.SystemMalfunction.ResponseCode(),
                    Description = ResponseEnum.BadRequest.DisplayName()
                });
            }
        }
    }
}
