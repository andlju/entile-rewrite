using Entile.TestApp.ViewModels;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entile.TestApp.Tests
{


    [TestClass]
    public class EntileTests : SilverlightTest
    {
        // Unless found in cache, Entile root API should be loaded
        [TestMethod]
        [Asynchronous]
        public void When_Initializing_Api_Root_Should_Be_Loaded()
        {
            EntileViewModel viewModel = new EntileViewModel();
            viewModel.RootApiCommand.Execute(null);

            viewModel.PropertyChanged += (s, e) =>
                                             {
                                                 if (e.PropertyName == "RegisterClientCommand")
                                                     TestComplete();
                                             };
        }


        // Enabling notifications should GET registration information
        // If unknown client, client registration should be PUT


    }
}