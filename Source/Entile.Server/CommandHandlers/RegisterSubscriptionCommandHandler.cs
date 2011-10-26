using System;
using CommonDomain.Persistence;
using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class RegisterSubscriptionCommandHandler : IMessageHandler<RegisterSubscriptionCommand>
    {
        private IRepository _repository;


        public RegisterSubscriptionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(RegisterSubscriptionCommand command)
        {
            var client = _repository.GetById<Client>(command.ClientId, int.MaxValue);

            client.RegisterSubscription(command.SubscriptionId, command.Kind, command.Uri,
                                           command.ExtendedInformation);

            _repository.Save(client, Guid.NewGuid(), null);
        }
    }
}