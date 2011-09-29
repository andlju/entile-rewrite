using System;
using Entile.Server.Commands;
using Entile.Server.ViewHandlers;

namespace Entile.Server
{
    public interface INotifier
    {
        void SendNotificationToAllClients(string title, string body);
    }

    public class Notifier : INotifier
    {
        private readonly IBus _messageBus;

        public Notifier(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void SendNotificationToAllClients(string title, string body)
        {
            var context = new EntileViews();
            foreach (var client in context.ClientViews)
            {
                var clientId = Guid.Parse(client.UniqueId);
                SendCommand(new SendToastNotificationCommand(clientId, Guid.NewGuid(), title, body, null, 3));
            }
        }

        private void SendCommand(ICommand command)
        {
            _messageBus.Publish(command);
        }

    }
}