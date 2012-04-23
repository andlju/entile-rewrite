using Entile.TestApp.ViewModels;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entile.TestApp.Tests
{
    [TestClass]
    public class EntileIntegrationTests : SilverlightTest
    {
        // Unless found in cache, Entile root API should be loaded
        [TestMethod]
        [Asynchronous]
        public void When_Initializing_Api_Root_Should_Be_Loaded()
        {
            EntileViewModel viewModel = new EntileViewModel();
            viewModel.RootApiQuery.Execute(null);

            viewModel.PropertyChanged += (s, e) =>
                                             {
                                                 if (e.PropertyName == "SubscribeCommand")
                                                     TestComplete();
                                             };
        }

        // When ClientQuery returns 404, client should be registered
        [TestMethod]
        public void When_ClientQuery_Returns_No_Client_It_Should_Call_Register()
        {
            EntileViewModel viewModel = new EntileViewModel();
        }

        // Enabling notifications should GET registration information
        // If unknown client, client registration should be PUT


    }
}