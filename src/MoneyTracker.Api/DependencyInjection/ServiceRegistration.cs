using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.Services;
using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Application.Validators.Transactions;
using MoneyTracker.Api.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using MoneyTracker.Application.Validators.Categories;
using MoneyTracker.Application.DTOs.Categories;
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
            services.AddInfrastructure(configuration);
            
            // Application services
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICategoryService, CategoryService>();

            // Kafka 
            services.AddSingleton<IMessageSerializer, JsonMessageSerializer>();
            services.Configure<KafkaConnectionOptions>(
            configuration.GetSection("Kafka:Connection"));
            services.AddSingleton<IMessageProducer, KafkaProducer>();

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<CategorySaveDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoryQueryDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoryPatchDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<TransactionSaveDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<TransactionQueryDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<TransactionPatchDtoValidator>();
            services.AddFluentValidationClientsideAdapters();

            // Filters
            services.AddScoped<ValidationFilterAttribute<CategorySaveDto>>();
            services.AddScoped<ValidationFilterAttribute<CategoryQueryDto>>();
            services.AddScoped<ValidationFilterAttribute<CategoryPatchDto>>();
            services.AddScoped<ValidationFilterAttribute<TransactionSaveDto>>();
            services.AddScoped<ValidationFilterAttribute<TransactionQueryDto>>();
            services.AddScoped<ValidationFilterAttribute<TransactionPatchDto>>();

            return services;
        }
    }
}
