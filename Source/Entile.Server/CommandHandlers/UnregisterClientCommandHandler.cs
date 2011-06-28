using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class UnregisterClientCommandHandler : ICommandHandler<UnregisterClientCommand>
    {
        private readonly IRepository<Client> _registrationRepository;

        public UnregisterClientCommandHandler(IRepository<Client> registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public void Execute(UnregisterClientCommand command)
        {
            var registration = _registrationRepository.GetById(command.UniqueId);

            if (registration != null)
            {
                registration.Unregister();
            }
            _registrationRepository.SaveChanges(registration);
        }

    }
}