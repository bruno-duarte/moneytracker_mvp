using MoneyTracker.Application;
using MoneyTracker.Api.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using MoneyTracker.Application.Validators.Categories;
using MoneyTracker.Messaging.Kafka.Configuration;
using MoneyTracker.Messaging.Abstractions;
using MoneyTracker.Messaging.Kafka.Internal.Producer;
using MoneyTracker.Messaging.Abstractions.Internal.Serialization;
using MoneyTracker.Infrastructure;

namespace MoneyTracker.Api.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddMoneyTrackerDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Infrastructure
            services.AddInfrastructure(configuration);

            // Application
            services.AddApplication(configuration);

            // Kafka 
            services.AddSingleton<IMessageSerializer, JsonMessageSerializer>();
            services.Configure<KafkaConnectionOptions>(
                configuration.GetSection("Kafka:Connection"));
            services.AddSingleton<IMessageProducer, KafkaProducer>();

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<CategorySaveDtoValidator>();
            services.AddFluentValidationClientsideAdapters();

            // Filters
            services.AddScoped(typeof(ValidationFilterAttribute<>));

            return services;
        }
    }
}
