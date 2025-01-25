using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OrderProcessing.Api.Endpoints.Orders
{
	public class OrdersModule : IEndpointsModule
	{
		public void RegisterEndpoints(IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("/api/orders")
				.WithTags("Orders")
				.WithOpenApi();

			//group.MapPost("/", CreateOrder.Handle)
			//	.WithName("PostOrder")
			//	.WithDescription("Place a new order.");
		}
	}

	public static class CreateOrder
	{
		//public static async Task<IResult> Handle(
		//	[FromBody] CreateOrderRequest request
		//)
		//{

		//}
	}
}
