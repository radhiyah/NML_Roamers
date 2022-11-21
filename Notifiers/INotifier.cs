using Nml.Refactor.Me.Dependencies;
using System.Threading.Tasks;

namespace Nml.Refactor.Me.Notifiers
{
	public interface INotifier
	{
		
		Task SmsNotification(NotificationModel message);

		Task SlackNotification(NotificationModel message);

		Task EmailNotification(NotificationModel message);

		Task TeamsNotification(NotificationModel message);
	}
}
