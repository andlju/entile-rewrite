using System;
using CommonDomain.Persistence;
using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class UnregisterSubscriptionCommandHandler : IMessageHandler<UnregisterSubscriptionCommand>
    {
        private readonly IRepository _repository;

        public UnregisterSubscriptionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(UnregisterSubscriptionCommand command)
        {
            var client = _repository.GetById<Client>(command.ClientId, int.MaxValue);

            client.UnregisterSubscription(command.SubscriptionId);
            
            _repository.Save(client, Guid.NewGuid(), null);
        }
    }
}