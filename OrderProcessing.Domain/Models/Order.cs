namespace OrderProcessing.Domain.Models
{
	public enum OrderStatus
	{
		Created,
		Processing,
		Completed,
		Failed,
	}

	public class Order
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string CustomerName { get; set; }
		public OrderStatus Status { get; set; } = OrderStatus.Created;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

		public Order(string customerName)
		{
			CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
		}

		public void MarkProcessing() => Status = OrderStatus.Processing;
		public void MarkCompleted() => Status = OrderStatus.Completed;
		public void MarkFailed() => Status = OrderStatus.Failed;
	}
}
