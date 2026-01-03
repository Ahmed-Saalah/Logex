using Logex.API.Constants;
using Logex.API.Dtos.PaymentDtos;
using Logex.API.Models;
using Logex.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        private readonly IPaymentService _paymentService;
        private readonly IStripePaymentService _stripePaymentService;

        public PaymentController(
            IShipmentService shipmentService,
            IPaymentService paymentService,
            IStripePaymentService stripePaymentService
        )
        {
            _shipmentService = shipmentService;
            _paymentService = paymentService;
            _stripePaymentService = stripePaymentService;
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<PaymentInitiationResponse>> InitiateCheckout(
            [FromBody] InitiatePaymentDto request
        )
        {
            var shipment = await _shipmentService.GetByIdAsync(request.ShipmentId);
            if (shipment == null)
            {
                return NotFound(new { Message = "Shipment not found." });
            }

            if (shipment.Status != ShipmentStatus.Pending)
            {
                return BadRequest(new { Message = "This shipment is already processed or paid." });
            }

            var totalAmount = shipment.TotalCost;

            var newPayment = new Payment
            {
                ShipmentId = shipment.Id,
                Amount = totalAmount,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UserId = shipment.UserId,
            };

            var createdPayment = await _paymentService.CreatePaymentAsync(newPayment);

            shipment.PaymentId = createdPayment.Id;
            //await _shipmentService.UpdateAsync(shipment);

            var origin = $"{Request.Scheme}://{Request.Host}";
            var checkoutUrl = await _stripePaymentService.CreateCheckoutSessionAsync(
                createdPayment,
                shipment,
                origin
            );

            return Ok(
                new PaymentInitiationResponse
                {
                    PaymentId = createdPayment.Id,
                    Amount = createdPayment.Amount,
                    CheckoutUrl = checkoutUrl,
                }
            );
        }
    }
}
