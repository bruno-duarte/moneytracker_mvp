using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure;
using MoneyTracker.Infrastructure.Kafka;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace MoneyTracker.Api.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddMoneyTrackerDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<MoneyTrackerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            // Repositories
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Application services
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICategoryService, CategoryService>();

            // Kafka 
            services.AddSingleton<ProducerWrapper>();
            services.AddSingleton<IEventBus, KafkaEventBus>();

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Program>();

            return services;
        }
    }
}
