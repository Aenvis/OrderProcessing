namespace OrderProcessing.Api.Endpoints.Orders
{
	public record CreateOrderResponse(Guid OrderId, Guid JobId, string Status, DateTime CreatedAt);
}
