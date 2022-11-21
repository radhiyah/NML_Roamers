using System;
using System.Threading.Tasks;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class SmsNotifier : INotifier
	{
		private readonly IStringMessageBuilder _messageBuilder;
		private readonly IOptions _options;
		private readonly ILogger _logger = LogManager.For<SmsNotifier>();

		public SmsNotifier(IStringMessageBuilder messageBuilder, IOptions options)
		{
			_messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}
		
		public async Task Notify(NotificationMessage message)
		{
			//Complete after refactoring inheritance. Use "SmsApiClient"
		}
	}
}
