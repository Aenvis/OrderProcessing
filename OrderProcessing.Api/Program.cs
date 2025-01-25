
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Endpoints;
using OrderProcessing.Api.Endpoints.Orders;
using OrderProcessing.Infrastructure;
using OrderProcessing.Infrastructure.Data;

namespace OrderProcessing.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddInfrastructure(builder.Configuration);

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/", () => "Hello World1");

            app.MapEndpointModule<OrdersModule>();

            app.Run();
        }
    }
}
