using FluentResults;
using OrderProcessing.Domain.Models;

namespace OrderProcessing.Application.Interfaces
{
	public interface IJobRepository
	{
		Task<Result<Job>> GetJobByIdAsync(Guid jobId);
		Task<Result> AddJobAsync(Job job);
		Task<Result> UpdateJobAsync(Job job);
		Task<Result<List<Job>>> GetPendingJobsAsync(int maxJobs);
	}
}
