using FluentResults;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Interfaces;
using OrderProcessing.Domain.Models;

namespace OrderProcessing.Infrastructure.Data
{
	public class JobRepository : IJobRepository
	{
		private readonly AppDbContext _context;

		public JobRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Job>> GetJobByIdAsync(Guid jobId)
		{
			try
			{
				var job = await _context.Jobs
					.Include(j => j.Order)
					.FirstOrDefaultAsync(j => j.Id == jobId);

				return job == null ? Result.Fail("Job not found.") : Result.Ok(job);
			}
			catch (Exception ex)
			{
				return Result.Fail($"Error retrieving job: {ex.Message}");
			}
		}

		public async Task<Result> AddJobAsync(Job job)
		{
			try
			{
				await _context.Jobs.AddAsync(job);
				await _context.SaveChangesAsync();
				return Result.Ok();
			}
			catch (Exception ex)
			{
				return Result.Fail($"Error adding job: {ex.Message}");
			}
		}

		public async Task<Result> UpdateJobAsync(Job job)
		{
			try
			{
				_context.Jobs.Update(job);
				await _context.SaveChangesAsync();
				return Result.Ok();
			}
			catch (Exception ex)
			{
				return Result.Fail($"Error updating job: {ex.Message}");
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
					.Include(j => j.Order)
					.ToListAsync();

				return Result.Ok(jobs);
			}
			catch (Exception ex)
			{
				return Result.Fail($"Error retrieving pending jobs: {ex.Message}");
			}
		}
	}
}
