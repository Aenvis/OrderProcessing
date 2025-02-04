using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.Application.Interfaces;
using OrderProcessing.Infrastructure.Data;
using OrderProcessing.Infrastructure.Messaging;

namespace OrderProcessing.Infrastructure
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddInfrastructure(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddDbContext<AppDbContext>(o =>
				o.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

			services.AddAWSService<IAmazonSQS>();
			services.AddScoped<IQueueService, SqsQueueService>();

			return services;
		}
	}
}
