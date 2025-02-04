using Amazon.SQS;
using Amazon.SQS.Model;
using OrderProcessing.Application.Interfaces;
using OrderProcessing.Domain.Models;

namespace OrderProcessing.Worker
{
	public class OrderProcessingWorker : BackgroundService
	{
		private readonly ILogger<OrderProcessingWorker> _logger;
		private readonly IAmazonSQS _sqsClient;
		private readonly IJobRepository _jobRepository;
		private readonly string _queueUrl;


		public OrderProcessingWorker(ILogger<OrderProcessingWorker> logger, IAmazonSQS sqsClient,
			IJobRepository jobRepository, IQueueService queueService, IConfiguration configuration)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_sqsClient = sqsClient ?? throw new ArgumentNullException(nameof(sqsClient));
			_jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
			_queueUrl = configuration["Services:QueueUrl"] ?? throw new ArgumentNullException("Services:QueueUrl is not configured");
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Start order processing worker.");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					var receiveRequest = new ReceiveMessageRequest
					{
						QueueUrl = _queueUrl,
						MaxNumberOfMessages = 10,
						WaitTimeSeconds = 20,
					};

					var receiveResponse = await _sqsClient.ReceiveMessageAsync(receiveRequest, stoppingToken);

					if (receiveResponse.Messages.Any())
					{
						_logger.LogInformation("Received {count} messages from SQS.", receiveResponse.Messages.Count);


						foreach (var message in receiveResponse.Messages)
						{
							await ProcessMessageAsync(message, stoppingToken);
						}
					}
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Error occured while receiving messages from SQS.");
				}
			}

			_logger.LogInformation("Stop order processing worker.");
		}

		private async Task ProcessMessageAsync(Message message, CancellationToken stoppingToken)
		{
			try
			{
				if (!Guid.TryParse(message.Body, out var jobId))
				{
					_logger.LogWarning("Invalid jobId, messageId: {messageId}", message.MessageId);
					return;
				}

				_logger.LogInformation("Processing job {jobId}", jobId);
				var jobResult = await _jobRepository.GetJobByIdAsync(jobId);
				if (jobResult.IsFailed)
				{
					_logger.LogWarning("Job {jobId} not found or error: {error}", jobId, jobResult.ToString());
					return;
				}

				var job = jobResult.Value;

				job.MarkProcessing();
				var updateResult = await _jobRepository.UpdateJobAsync(job);
				if (updateResult.IsFailed)
				{
					_logger.LogWarning("Failed to update job {jobId}. Error: {error}", jobId, updateResult.ToString());
					return;
				}

				await ProcessJobAsync(job, stoppingToken);             
				job.MarkCompleted();
				var completeResult = await _jobRepository.UpdateJobAsync(job);
				if (completeResult.IsFailed)
				{
					_logger.LogWarning("Failed to update job {jobId}. Error: {error}", jobId, completeResult.ToString());
					return;
				}

				_logger.LogInformation("Job {jobId} processed successfully.", jobId);
				await DeleteMessageAsync(message, stoppingToken);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error while processing job.");
			}
		}

		private async Task ProcessJobAsync(Job job, CancellationToken stoppingToken)
		{
			_logger.LogInformation("Simualting processing job for job {jobId}", job.Id);
			await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
		}

		private async Task DeleteMessageAsync(Message message, CancellationToken stoppingToken)
		{
			try
			{
				var deleteRequest = new DeleteMessageRequest
				{
					QueueUrl = _queueUrl,
					ReceiptHandle = message.ReceiptHandle,
				};
				await _sqsClient.DeleteMessageAsync(deleteRequest, stoppingToken);
				_logger.LogInformation("Deleted message {messageId} from SQS", message.MessageId);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error while deleting message {messageId} from SQS", message.MessageId);
			}
		}
	}
}
