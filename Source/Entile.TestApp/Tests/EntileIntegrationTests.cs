using Entile.TestApp.Actions;
using Entile.TestApp.ViewModels;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entile.TestApp.Tests
{
    [TestClass]
    public class EntileIntegrationTests : SilverlightTest
    {
        MockWebClient WebClient = new MockWebClient();

        [TestInitialize]
        public void Initialize()
        {
            ApiQueryBase.WebClientFactory = new MockWebClientFactory() { WebClient = WebClient};
        }

        // Unless found in cache, Entile root API should be loaded
        [TestMethod]
        [Asynchronous]
        public void When_Initializing_RegisterCommand_Should_Be_Made_Available()
        {
            EntileViewModel viewModel = new EntileViewModel();

            viewModel.PropertyChanged += (s, e) =>
                                             {
                                                 if (e.PropertyName == "RegisterClientCommand")
                                                 {
                                                     TestComplete();
                                                 }
                                             };
        
            viewModel.Initialize();
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