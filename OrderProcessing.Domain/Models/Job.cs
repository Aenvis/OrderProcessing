namespace OrderProcessing.Domain.Models
{
	public enum JobStatus
	{
		Queued,
		Processing,
		Completed,
		Failed,
	}

	public class Job
	{
		public Guid Id { get; } = Guid.NewGuid();
		public Guid OrderId { get; }
		public JobStatus Status { get; private set; } = JobStatus.Queued;
		public int AttemptCount { get; private set; }
		public DateTime CreatedAt { get; } = DateTime.UtcNow;
		public DateTime LastAttemptAt { get; private set; }

		public Job(Guid orderId) => OrderId = orderId;

		public void MarkProcessing()
		{
			Status = JobStatus.Processing;
			LastAttemptAt = DateTime.UtcNow;
			AttemptCount++;
		}

		public void MarkCompleted() => Status = JobStatus.Completed;
		public void MarkFailed() => Status = AttemptCount >= 3 ? JobStatus.Failed : JobStatus.Queued;
	}
}
