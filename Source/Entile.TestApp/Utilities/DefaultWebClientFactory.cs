using Entile.TestApp.ViewModels;

namespace Entile.TestApp.Utilities
{
    public class DefaultWebClientFactory : IWebClientFactory
    {
        public ISimpleWebClient CreateWebClient()
        {
            return new SimpleWebClient();
        }
    }
}