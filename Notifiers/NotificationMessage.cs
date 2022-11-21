// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable 8618
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Nml.Refactor.Me.Notifiers
{
	
	public class NotificationMessage :INotifier
	{
		private readonly IMailMessageBuilder _messageBuilder;
		private readonly IOptions _options;
		private readonly ILogger _logger = LogManager.For<EmailNotifier>();
	

		public NotificationMessage(IMailMessageBuilder messageBuilder, IOptions options)
        {
			_messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		
		}
		
        public async Task EmailNotification(NotificationModel message)
        {
			var smtp = new SmtpClient(_options.Email.SmtpServer);
			smtp.Credentials = new NetworkCredential(_options.Email.UserName, _options.Email.Password);
			var mailMessage = _messageBuilder.CreateMessage(message);

			try
			{
				await smtp.SendMailAsync(mailMessage);
				_logger.LogTrace($"Message sent.");
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Failed to send message. {e.Message}");
				throw;
			}
		}

        public async Task SlackNotification(NotificationModel message)
        {
			var serviceEndPoint = new Uri(_options.Slack.WebhookUri);
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, serviceEndPoint);
			request.Content = new StringContent(
				_messageBuilder.CreateMessage(message).ToString(),
				Encoding.UTF8,
				"application/json");

			var response = await client.SendAsync(request);
			_logger.LogTrace($"Message sent. {response.StatusCode} -> {response.Content}");
		}

        public async Task SmsNotification(NotificationModel message)
        {
			var sms = new SmsApiClient(_options.Sms.ApiUri, _options.Sms.ApiKey);
			var smsMessage = _messageBuilder.CreateMessage(message);
            try
            {
				await sms.SendAsync(message.To, message.Body);
            }
			catch (AggregateException e)
			{
				foreach (var exception in e.Flatten().InnerExceptions)
					_logger.LogError(exception, $"Failed to send message. {exception.Message}");

				throw;
			}
		}

        public async Task TeamsNotification(NotificationModel message)
        {
			var serviceEndPoint = new Uri(_options.Teams.WebhookUri);
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, serviceEndPoint);
			request.Content = new StringContent(
				_messageBuilder.CreateMessage(message).ToString(),
				Encoding.UTF8,
				"application/json");
			try
			{
				var response = await client.SendAsync(request);
				_logger.LogTrace($"Message sent. {response.StatusCode} -> {response.Content}");
			}
			catch (AggregateException e)
			{
				foreach (var exception in e.Flatten().InnerExceptions)
					_logger.LogError(exception, $"Failed to send message. {exception.Message}");

				throw;
			}
		}
    }
}
