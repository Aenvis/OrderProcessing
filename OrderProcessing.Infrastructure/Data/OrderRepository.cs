using FluentResults;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Errors;
using OrderProcessing.Domain.Interfaces;
using OrderProcessing.Domain.Models;

namespace OrderProcessing.Infrastructure.Data
{
	public class OrderRepository : IOrderRepository
	{
		private readonly AppDbContext _context;

		public OrderRepository(AppDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<Result<Order>> GetOrderByIdAsync(Guid id)
		{
			try
			{
				var order = await _context.Orders
					.Include(o => o.Jobs)
					.FirstOrDefaultAsync(o => o.Id == id);

				return order is null
					? Result.Fail<Order>(DomainErrors.OrderNotFound(id))
					: Result.Ok(order);
			}
			catch (Exception e)
			{
				return Result.Fail<Order>(DomainErrors.DatabaseOperationFailed(e));
			}
		}

		public async Task<Result> AddOrderAsync(Order order)
		{
			try
			{
				await _context.Orders.AddAsync(order);
				await _context.SaveChangesAsync();
				return Result.Ok();
			}
			catch (Exception e)
			{
				return Result.Fail(DomainErrors.DatabaseOperationFailed(e));
			}
		}

		public async Task<Result> UpdateOrderAsync(Order order)
		{
			try
			{
				_context.Orders.Update(order);
				await _context.SaveChangesAsync();
				return Result.Ok();
			}
			catch (Exception e)
			{
				return Result.Fail(DomainErrors.DatabaseOperationFailed(e));
			}
		}

		public async Task<Result<List<Job>>> GetPendingJobsAsync(int maxJobs)
		{
			try
			{
				var jobs = await _context.Jobs
					.Where(j => j.Status == JobStatus.Queued)
					.OrderBy(j => j.CreatedAt)
					.Take(maxJobs)
					.ToListAsync();

				return Result.Ok(jobs);

			}
			catch (Exception e)
			{
				return Result.Fail<List<Job>>(DomainErrors.DatabaseOperationFailed(e));
			}
		}

		public async Task<Result<List<Order>>> GetAllOrdersAsync()
		{
			try
			{
				var orders = await _context.Orders
					.Include(o => o.Jobs)
					.OrderByDescending(o => o.CreatedAt)
					.ToListAsync();

				return Result.Ok(orders);
			}
			catch (Exception e)
			{
				return Result.Fail<List<Order>>(DomainErrors.DatabaseOperationFailed(e));
			}
		}

		public async Task<Result<List<Order>>> GetOrdersByStatusAsync(OrderStatus status)
		{
			try
			{
				var orders = await _context.Orders
					.Include(o => o.Jobs)
					.Where(o => o.Status == status)
					.OrderByDescending(o => o.CreatedAt)
					.ToListAsync();

				return Result.Ok(orders);
			}
			catch (Exception e)
			{
				return Result.Fail<List<Order>>(DomainErrors.DatabaseOperationFailed(e));
			}
		}
	}
}
