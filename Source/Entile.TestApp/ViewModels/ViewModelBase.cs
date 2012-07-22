using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AgFx;
using Entile.TestApp.Models;

namespace Entile.TestApp.ViewModels
{
    public abstract class ViewModelBase : NotifyPropertyChangedBase
    {
        private readonly Dictionary<string, Action<LinkModel>> _linkActionFactories = new Dictionary<string, Action<LinkModel>>();
        private readonly Dictionary<string, Action<CommandModel>> _commandActionFactories = new Dictionary<string, Action<CommandModel>>();

        protected void RegisterCommandAction(string name, Action<CommandModel> buildAction)
        {
            _commandActionFactories.Add(name, buildAction);
        }

        protected void RegisterLinkAction(string name, Action<LinkModel> buildAction)
        {
            _linkActionFactories.Add(name, buildAction);
        }

        protected void UpdateLinks(IEnumerable<LinkModel> links)
        {
            foreach (var link in links)
            {
                Action<LinkModel> linkFactory;
                if (_linkActionFactories.TryGetValue(link.Rel, out linkFactory))
                {
                    linkFactory(link);
                }
            }
        }

        protected void UpdateCommands(IEnumerable<CommandModel> commands)
        {
            foreach(var command in commands)
            {
                Action<CommandModel> commandFactory;
                if (_commandActionFactories.TryGetValue(command.Name, out commandFactory))
                {
                    commandFactory(command);
                }
            }
        }

    }
}