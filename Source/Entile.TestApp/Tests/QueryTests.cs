using Entile.TestApp.Actions;
using Entile.TestApp.ViewModels;
using Microsoft.Silverlight.Testing;
using Microsoft.Silverlight.Testing.UnitTesting.Metadata.VisualStudio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entile.TestApp.Tests
{

    [TestClass]
    public class QueryTests : SilverlightTest
    {
        private MockWebClientFactory _webClientFactory;
        private MockWebClient _webClient;

        [TestInitialize]
        public void Initialize()
        {
            _webClientFactory = new MockWebClientFactory();
            _webClientFactory.WebClient = _webClient = new MockWebClient();
            _webClient.PrepareResponse("/api", new WebResponseEventArgs("http://localhost:3852/api/", 200, "{ \"Links\" : [ { \"Rel\" : \"Register\", \"Uri\" : \"http://localhost:3852/api/register\"} ] }"));
            ApiQueryBase.WebClientFactory = _webClientFactory;
        }

        [TestMethod]
        public void RootApiQuery_Sets_Links()
        {
            var viewModel = new EntileViewModel();
            
            viewModel.RootApiQuery.Execute(null);
            
            Assert.IsNotNull(viewModel.RegisterClientCommand);
        }



    }
}