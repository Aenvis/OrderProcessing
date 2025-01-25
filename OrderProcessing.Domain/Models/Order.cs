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
		public Guid Id { get; private set; } = Guid.NewGuid();
		public string CustomerName { get; private set; }
		public OrderStatus Status { get; private set; } = OrderStatus.Created;
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		public Order(string customerName)
		{
			CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
		}

		public void MarkProcessing() => Status = OrderStatus.Processing;
		public void MarkCompleted() => Status = OrderStatus.Completed;
		public void MarkFailed() => Status = OrderStatus.Failed;
	}
}
