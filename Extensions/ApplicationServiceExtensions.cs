using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Logex.API.Data;
using Logex.API.Repository.Implementations;
using Logex.API.Repository.Interfaces;
using Logex.API.Services.Implementations;
using Logex.API.Services.Interfaces;
using Logex.API.Settings;
using Microsoft.EntityFrameworkCore;

namespace Logex.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            // Database Setup
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("ConnectionString"))
            );

            // Stripe Configuration
            services.AddOptions<StripeOptions>().Bind(config.GetSection("Stripe"));
            services.AddSingleton(resolver =>
                resolver
                    .GetRequiredService<Microsoft.Extensions.Options.IOptions<StripeOptions>>()
                    .Value
            );
            services.AddScoped<IStripePaymentService, StripePaymentService>();

            // Repositories
            services.AddScoped<IShipmentRepository, ShipmentRepository>();
            services.AddScoped<IShipmentMethodRepository, ShipmentMethodRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPricingRepository, PricingRepository>();
            services.AddScoped<IZoneRepository, ZoneRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IZoneRateRepository, ZoneRateRepository>();
            // Domain Services
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IShipmentMethodService, ShipmentMethodService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenManagement, TokenManagement>();
            services.AddScoped<IUserManagement, UserManagement>();
            services.AddScoped<IPricingService, PricingService>();

            // Controllers & JSON Options
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // CORS & Swagger
            services
                .AddCors(options =>
                {
                    options.AddPolicy(
                        "MyPolicy",
                        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                    );
                })
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddSwaggerDocumentation();

            return services;
        }
    }
}
