using System;
using CommonDomain.Persistence;
using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class RegisterClientCommandHandler : IMessageHandler<RegisterClientCommand>
    {
        private readonly IRepository _repository;

        public RegisterClientCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(RegisterClientCommand command)
        {
            var client = _repository.GetById<Client>(command.ClientId, int.MaxValue);

            if (client.Id != Guid.Empty)
            {
                client.UpdateRegistration(command.NotificationChannel);
            } 
            else
            {
                client = new Client(command.ClientId, command.NotificationChannel); 
            }

            _repository.Save(client, Guid.NewGuid(), null);
        }
    }
}