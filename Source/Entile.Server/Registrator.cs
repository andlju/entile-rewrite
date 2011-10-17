using System;
using System.Collections.Generic;
using Entile.Server.Commands;

namespace Entile.Server
{
    public class Registrator : IRegistrator
    {
        private readonly IBus _messageBus;

        public Registrator(IBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Register(Guid uniqueId, string notificationChannel)
        {
            SendCommand(
                new RegisterClientCommand(
                    uniqueId, 
                    notificationChannel));

        }

        public void Unregister(Guid uniqueId)
        {
            SendCommand(
                new UnregisterClientCommand(uniqueId));
        }

        private void SendCommand(ICommand command)
        {
            _messageBus.Publish(command);
        }
    }
}