using Amazon.SQS;
using Amazon.SQS.Model;
using FluentResults;
using Microsoft.Extensions.Configuration;
using OrderProcessing.Domain.Errors;
using OrderProcessing.Domain.Interfaces;

namespace OrderProcessing.Infrastructure.Messaging
{
	public class SqsQueueService : IQueueService
	{
		private readonly IAmazonSQS _sqs;
		private readonly string _queueUrl;

		public SqsQueueService(IAmazonSQS sqs, IConfiguration config)
		{
			_sqs = sqs ?? throw new ArgumentNullException(nameof(sqs));
			_queueUrl = config["AWS:QueueUrl"];
		}

		public async Task<Result> EnqueueJobAsync(Guid jobId)
		{
			try
			{
				await _sqs.SendMessageAsync(new SendMessageRequest
				{
					QueueUrl = _queueUrl,
					MessageBody = jobId.ToString(),
				});

				return Result.Ok();
			}
			catch (Exception e)
			{
				return Result.Fail(DomainErrors.QueueOperationFailed(e));
			}
		}

		public async Task<Result> RequeueJobAsync(Guid jobId)
		{
			throw new NotImplementedException();
		}
	}
}
