using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.Notifiers;

namespace Nml.Refactor.Me.MessageBuilders
{
	public interface IMessageBuilder<out TMessageType>
	{
		TMessageType CreateMessage(NotificationModel message);
	}
}
