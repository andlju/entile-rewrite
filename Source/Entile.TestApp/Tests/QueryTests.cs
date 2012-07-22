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
            _webClient.PrepareResponse("http://localhost:6776/api/", new WebResponseEventArgs("http://localhost:6776/api/", 200, "{ \"Commands\" : [ { \"Name\" : \"register\", \"Uri\" : \"http://localhost:6776/register\", \"Method\" : \"POST\"} ] }"));
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