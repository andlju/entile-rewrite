using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AgFx;

namespace Entile.TestApp.ViewModels
{

    public class LinkModel
    {
        public string Rel;
        public string Uri;
    }

    public abstract class ActionableObject : NotifyPropertyChangedBase
    {
        private ObservableCollection<ICommand> _availableCommands = new ObservableCollection<ICommand>();
        private IEnumerable<LinkModel> _linkModels = new LinkModel[0];

        public void LoadLinks(IEnumerable<LinkModel> linkModels)
        {
            // Remove all old commands
            foreach(var linkmodel in _linkModels)
            {
                RemoveCommand(linkmodel.Rel);
            }

            // Replace with new
            _linkModels = linkModels.ToArray();
            foreach (var linkModel in _linkModels)
            {
                BuildCommand(linkModel.Rel, linkModel.Uri);
            }
        }

        protected abstract ICommand BuildCommand(string rel, string uri);
        protected abstract void RemoveCommand(string rel);

    }
}