using FluentResults;
using OrderProcessing.Domain.Models;

namespace OrderProcessing.Domain.Interfaces
{
	public interface IOrderRepository
	{
		Task<Result<Order>> GetOrderByIdAsync(Guid id);
		Task<Result> AddOrderAsync(Order order);
		Task<Result> UpdateOrderAsync(Order order);
		Task<Result<List<Job>>> GetPendingJobsAsync(int maxJobs);
		Task<Result<List<Order>>> GetAllOrdersAsync();
		Task<Result<List<Order>>> GetOrdersByStatusAsync(OrderStatus status);
	}
}
