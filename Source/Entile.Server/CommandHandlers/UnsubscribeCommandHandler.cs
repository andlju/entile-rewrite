using System;
using CommonDomain.Persistence;
using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class UnsubscribeCommandHandler : IMessageHandler<UnsubscribeCommand>
    {
        private readonly IRepository _repository;

        public UnsubscribeCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(UnsubscribeCommand command)
        {
            var client = _repository.GetById<Client>(command.ClientId, int.MaxValue);

            client.Unsubscribe(command.SubscriptionId);
            
            _repository.Save(client, Guid.NewGuid(), null);
        }
    }
}