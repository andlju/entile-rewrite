using System;
using System.IO;
using Entile.TestApp.Models;
using Entile.TestApp.ViewModels;
using Newtonsoft.Json;

namespace Entile.TestApp.Actions
{
    public class RootApiQuery : ApiQueryBase
    {

        public RootApiQuery(IViewContext viewContext, string uri)
            : base(viewContext, uri)
        {
        }

    }
}