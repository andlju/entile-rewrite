using System.Collections.Generic;

namespace Entile.NancyHost.Api.ViewModel
{
    public class LinkViewModel
    {
        public string Uri;
        public string Rel;
    }

    public class ViewModelBase
    {
        public List<LinkViewModel> Links;
    }
}