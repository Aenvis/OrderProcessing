using FluentResults;

namespace OrderProcessing.Domain.Errors
{
	public static class DomainErrors
	{
		#region Order Errors
		public static OrderNotFoundError OrderNotFound(Guid id)
			=> new OrderNotFoundError(id);

		public class OrderNotFoundError : Error
		{
			public OrderNotFoundError(Guid orderId) : base($"Order {orderId} not found")
			{
				WithMetadata("ErrorCode", "ORD-404");
			}
		}
		#endregion

		#region Database Errors
		public static DatabaseError DatabaseOperationFailed(Exception e, string message = "Database operation failed")
		=> new DatabaseError(e, message);

		public class DatabaseError : Error
		{
			public DatabaseError(Exception e, string message) : base(message)
			{
				WithMetadata("Exception", e.Message);
				WithMetadata("ErrorCode", "DB-500");
			}
		}
		#endregion

		#region Queue Errors
		public static QueueError QueueOperationFailed(Exception e, string message = "Queue operation failed")
		=> new QueueError(e, message);

		public class QueueError : Error
		{
			public QueueError(Exception e, string message) : base(message)
			{
				WithMetadata("Exception", e.Message);
				WithMetadata("ErrorCode", "QUEUE-500");
			}
		}
		#endregion
	}
}
