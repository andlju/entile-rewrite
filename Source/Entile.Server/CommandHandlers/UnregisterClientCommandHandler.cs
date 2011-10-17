using System;
using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class UnregisterClientCommandHandler : IMessageHandler<UnregisterClientCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public UnregisterClientCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(UnregisterClientCommand command)
        {
            var client = _clientRepository.GetById(command.ClientId);

            if (client != null)
            {
                client.Unregister();
            } 
            else
            {
                throw new InvalidOperationException(string.Format("Unknown client {0}", command.ClientId));
            }
            _clientRepository.SaveChanges(client);
        }

    }
}