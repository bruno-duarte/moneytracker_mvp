using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Application.Services;
using MoneyTracker.Application.Services.Interfaces;

namespace MoneyTracker.Application
{
	public static class DependencyInjection
	{
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Services
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
	}
}