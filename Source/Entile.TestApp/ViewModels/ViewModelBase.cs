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
        private readonly Dictionary<string, Action<LinkModel>> _actionFactories = new Dictionary<string, Action<LinkModel>>();

        protected void RegisterAction(string name, Action<LinkModel> buildAction)
        {
            _actionFactories.Add(name, buildAction);
        }

        protected void UpdateLinks(IEnumerable<LinkModel> links)
        {
            foreach (var link in links)
            {
                Action<LinkModel> linkFactory;
                if (_actionFactories.TryGetValue(link.Rel, out linkFactory))
                {
                    linkFactory(link);
                }
            }
        }

    }
}