using System;
using CommonDomain.Persistence;
using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class SubscribeCommandHandler : IMessageHandler<SubscribeCommand>
    {
        private IRepository _repository;


        public SubscribeCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(SubscribeCommand command)
        {
            var client = _repository.GetById<Client>(command.ClientId, int.MaxValue);

            client.Subscribe(command.SubscriptionId, command.Kind, command.Uri,
                                           command.ExtendedInformation);

            _repository.Save(client, Guid.NewGuid(), null);
        }
    }
}