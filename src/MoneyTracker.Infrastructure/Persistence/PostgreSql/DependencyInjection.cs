using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Infrastructure.Persistence.Common;

namespace MoneyTracker.Infrastructure.Persistence.PostgreSql
{
	public static class DependencyInjection
	{
        public static IServiceCollection AddPostgreSql(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<PostgreSqlDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IMoneyTrackerDbContext>(
                p => p.GetRequiredService<PostgreSqlDbContext>());

            services.AddScoped<IUnitOfWork>(
                p => p.GetRequiredService<PostgreSqlDbContext>());
            return services;
        }
	}
}