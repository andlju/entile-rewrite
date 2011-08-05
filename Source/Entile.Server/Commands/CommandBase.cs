using System;

namespace Entile.Server.Commands
{
    public class CommandBase : ICommand
    {
        public long Timestamp { get; set; }

        public CommandBase()
        {
            Timestamp = DateTime.Now.Ticks;
        }
    }
}