using System;
using CommonDomain.Persistence;
using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class UnregisterClientCommandHandler : IMessageHandler<UnregisterClientCommand>
    {
        private readonly IRepository _repository;

        public UnregisterClientCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(UnregisterClientCommand command)
        {
            var client = _repository.GetById<Client>(command.ClientId, int.MaxValue);

            if (client.Id != Guid.Empty)
            {
                client.Unregister();
            } 
            else
            {
                throw new InvalidOperationException(string.Format("Unknown client {0}", command.ClientId));
            }
            _repository.Save(client, Guid.NewGuid(), null);
        }

    }
}