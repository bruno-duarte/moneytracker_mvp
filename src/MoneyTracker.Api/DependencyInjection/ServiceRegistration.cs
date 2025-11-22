using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure;
using MoneyTracker.Infrastructure.Kafka;
using MoneyTracker.Infrastructure.Repositories;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.Services;
using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Application.Validators.Transactions;
using MoneyTracker.Api.Filters;
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
            services.AddValidatorsFromAssemblyContaining<TransactionSaveDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<TransactionQueryDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<TransactionPatchDtoValidator>();
            services.AddFluentValidationClientsideAdapters();

            // Filters
            services.AddScoped<ValidationFilterAttribute<TransactionSaveDto>>();
            services.AddScoped<ValidationFilterAttribute<TransactionQueryDto>>();
            services.AddScoped<ValidationFilterAttribute<TransactionPatchDto>>();

            return services;
        }
    }
}
