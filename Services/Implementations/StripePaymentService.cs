using Logex.API.Models;
using Logex.API.Services.Interfaces;
using Logex.API.Settings;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Logex.API.Services.Implementations
{
    public class StripePaymentService : IStripePaymentService
    {
        private readonly StripeOptions _stripeOptions;

        public StripePaymentService(IOptions<StripeOptions> stripeOptions)
        {
            _stripeOptions = stripeOptions.Value;
            StripeConfiguration.ApiKey = _stripeOptions.SecretKey;
        }

        public async Task<string> CreateCheckoutSessionAsync(
            Payment payment,
            Shipment shipment,
            string originUrl
        )
        {
            var options = new SessionCreateOptions
            {
                Mode = "payment",
                ClientReferenceId = shipment.Id.ToString(),
                SuccessUrl = $"{originUrl}/payment-success?id={shipment.Id}", // Frontend Route
                CancelUrl = $"{originUrl}/payment-failed?id={shipment.Id}",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Logex Shipment #{shipment.TrackingNumber}",
                                Description = $"Delivery via {shipment.ShipmentMethod?.Name}",
                            },
                            UnitAmount = (long)(payment.Amount * 100),
                        },
                        Quantity = 1,
                    },
                },
                Metadata = new Dictionary<string, string>
                {
                    { "PaymentId", payment.Id.ToString() },
                    { "ShipmentId", shipment.Id.ToString() },
                    { "UserId", shipment.UserId.ToString() },
                },
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}
