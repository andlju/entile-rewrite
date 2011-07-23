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

        public void Register(string uniqueId, string notificationChannel)
        {
            SendCommand(
                new RegisterClientCommand(
                    uniqueId, 
                    notificationChannel));

        }

        public void Unregister(string uniqueId)
        {
            SendCommand(
                new UnregisterClientCommand(uniqueId));
        }

        public void SetExtendedInformation(string uniqueId, string key, string value)
        {
            SendCommand(
                new SetExtendedInformationItemCommand(uniqueId, key, value));
        }

        public void RemoveExtendedInformation(string uniqueId, string key)
        {
            SendCommand(
                new RemoveExtendedInformationItemCommand(uniqueId, key));
        }

        public void RemoveAllExtendedInformation(string uniqueId)
        {
            SendCommand(
                new RemoveAllExtendedInformationItemsCommand(uniqueId));
        }

        private void SendCommand(ICommand command)
        {
            _messageBus.Publish(command);
        }
    }
}