using System.ComponentModel.DataAnnotations;

namespace OrderProcessing.Api.Endpoints.Orders
{
	public record CreateOrderRequest(
		[Required(ErrorMessage = "Customer name is required")]
		[StringLength(100, MinimumLength = 2, ErrorMessage = "Customer name must be between 2-100 characters")]
		string CustomerName,

		[Required(ErrorMessage = "At least one order item is required")]
		[MinLength(1, ErrorMessage = "At least one order item is required")]
		List<OrderItemRequest> Items);

	public record OrderItemRequest(
		[Required(ErrorMessage = "Product ID is required")]
		Guid ProductId,

		[Required(ErrorMessage = "Product name is required")]
		[StringLength(50, ErrorMessage = "Product name cannot exceed 50 characters")]
		string ProductName,

		[Range(1, 100, ErrorMessage = "Quantity must be between 1-100")]
		int Quantity,

		[Range(0.01, 1_000_000, ErrorMessage = "Unit price must be between 0.01 - 1,000,000")]
		decimal UnitPrice
	);
}
