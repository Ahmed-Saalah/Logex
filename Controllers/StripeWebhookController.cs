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
        private readonly ILogger<StripeWebhookController> _logger;

        public StripeWebhookController(
            IOptions<StripeOptions> stripeOptions,
            IPaymentService paymentService,
            IShipmentRepository shipmentRepository,
            ILogger<StripeWebhookController> logger
        )
        {
            _webhookSecret = stripeOptions.Value.WebhookSecret;
            _paymentService = paymentService;
            _shipmentRepository = shipmentRepository;
            _logger = logger;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _webhookSecret
                );
            }
            catch (StripeException ex)
            {
                _logger.LogWarning(ex, "Invalid Stripe webhook signature");
                return BadRequest(ex.Message);
            }
            var paymentIntentService = new PaymentIntentService();

            _logger.LogInformation(
                "Stripe webhook received. EventId: {EventId}, Type: {EventType}",
                stripeEvent.Id,
                stripeEvent.Type
            );

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    await HandlePaymentSucceeded(stripeEvent);
                    break;

                case EventTypes.PaymentIntentPaymentFailed:
                    await HandlePaymentFailed(stripeEvent);
                    break;
            }

            return Ok();
        }

        private async Task HandlePaymentSucceeded(Event stripeEvent)
        {
            var intent = stripeEvent.Data.Object as PaymentIntent;
            if (intent == null)
            {
                return;
            }

            var paymentIdStr = intent.Metadata.GetValueOrDefault("PaymentId");
            if (!int.TryParse(paymentIdStr, out var paymentId))
            {
                return;
            }

            var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
            if (payment == null || payment.Status == PaymentStatus.Paid)
            {
                return;
            }

            payment.Status = PaymentStatus.Paid;
            payment.Reference = intent.Id;

            await _paymentService.UpdatePaymentAsync(payment.Id, payment);

            if (payment.ShipmentId.HasValue)
            {
                var shipment = await _shipmentRepository.GetByIdAsync(payment.ShipmentId.Value);
                if (shipment != null)
                {
                    shipment.Status = ShipmentStatus.Processing;
                    await _shipmentRepository.UpdateAsync(shipment);
                }
            }
        }

        private async Task HandlePaymentFailed(Event stripeEvent)
        {
            var intent = stripeEvent.Data.Object as PaymentIntent;
            if (intent == null)
            {
                return;
            }

            var paymentIdStr = intent.Metadata.GetValueOrDefault("PaymentId");
            if (!int.TryParse(paymentIdStr, out var paymentId))
            {
                return;
            }

            var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
            if (payment == null)
            {
                return;
            }

            if (payment.Status == PaymentStatus.Failed)
            {
                return;
            }

            payment.Status = PaymentStatus.Failed;
            await _paymentService.UpdatePaymentAsync(payment.Id, payment);
        }
    }
}
