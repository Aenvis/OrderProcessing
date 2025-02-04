using FluentResults;

namespace OrderProcessing.Application.Interfaces
{
	public interface IQueueService
	{
		Task<Result> EnqueueJobAsync(Guid jobId);
	}
}
