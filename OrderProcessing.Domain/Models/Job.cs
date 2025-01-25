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
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid OrderId { get; set; }
		public virtual Order Order { get; set; }
		public JobStatus Status { get; set; } = JobStatus.Queued;
		public int AttemptCount { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime LastAttemptAt { get; set; }

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
