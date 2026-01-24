using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Domain.Interfaces.Repositories;
using MoneyTracker.Infrastructure.Persistence.PostgreSql;
using MoneyTracker.Infrastructure.Repositories.EfCore;

namespace MoneyTracker.Infrastructure
{
	public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddPostgreSql(configuration);

            // Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}