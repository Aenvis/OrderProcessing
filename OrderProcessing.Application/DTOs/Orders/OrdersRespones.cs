namespace OrderProcessing.Application.DTOs.Orders
{
	public record CreateOrderResponse(
		Guid OrderId,
		Guid JobId,
		string Status,
		DateTime CreatedAt);

	public record GetOrderResponse(
		Guid Id,
		string CustomerName,
		string Status,
		DateTime CreatedAt,
		List<JobResponse> Jobs);

	public record CreateJobResponse(
		Guid JobId,
		Guid OrderId,
		string Status,
		DateTime CreatedAt);

	public record GetJobResponse(
		Guid Id,
		Guid OrderId,
		string Status,
		int AttemptCount,
		DateTime CreatedAt,
		DateTime? LastAttemptAt,
		string? ErrorDetails);

	public record JobResponse(
		Guid Id,
		string Status,
		int AttemptCount,
		DateTime CreatedAt);
}
