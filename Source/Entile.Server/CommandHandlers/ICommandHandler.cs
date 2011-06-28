using Entile.Server.Commands;

namespace Entile.Server.CommandHandlers
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}