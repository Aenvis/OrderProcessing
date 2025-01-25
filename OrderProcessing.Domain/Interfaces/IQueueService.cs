using FluentResults;

namespace OrderProcessing.Domain.Interfaces
{
	public interface IQueueService
	{
		Task<Result> EnqueueJobAsync(Guid jobId);
		Task<Result> RequeueJobAsync(Guid jobId);
	}
}
