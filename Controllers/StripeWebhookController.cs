using Logex.API.Constants;
using Logex.API.Repository.Interfaces;
using Logex.API.Services.Interfaces;
using Logex.API.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.Events;

namespace Logex.API.Controllers
{
    [Route("api/webhooks")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly string _webhookSecret;
        private readonly IPaymentService _paymentService;
        private readonly IShipmentRepository _shipmentRepository;

        public StripeWebhookController(
            IOptions<StripeOptions> stripeOptions,
            IPaymentService paymentService,
            IShipmentRepository shipmentRepository
        )
        {
            _webhookSecret = stripeOptions.Value.WebhookSecret;
            _paymentService = paymentService;
            _shipmentRepository = shipmentRepository;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _webhookSecret
                );

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;

                    if (
                        session?.Metadata != null
                        && session.Metadata.TryGetValue("PaymentId", out var paymentIdStr)
                    )
                    {
                        int paymentId = int.Parse(paymentIdStr);
                        await FulfillOrderAsync(paymentId, session.PaymentIntentId);
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }

        private async Task FulfillOrderAsync(int paymentId, string stripeTransactionId)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
            if (payment != null)
            {
                payment.Status = PaymentStatus.Completed;
                payment.Reference = stripeTransactionId;
                await _paymentService.UpdatePaymentAsync(payment.Id, payment);

                var shipment = await _shipmentRepository.GetByIdAsync(payment.ShipmentId!.Value);
                if (shipment != null)
                {
                    shipment.Status = ShipmentStatus.Processing;
                    await _shipmentRepository.UpdateAsync(shipment);
                }
            }
        }
    }
}
