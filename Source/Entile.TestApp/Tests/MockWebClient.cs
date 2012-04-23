using System;
using System.Collections.Generic;
using Entile.TestApp.ViewModels;

namespace Entile.TestApp.Tests
{
    public class MockWebClientFactory : IWebClientFactory
    {
        public MockWebClient WebClient { get; set; }

        public ISimpleWebClient CreateWebClient()
        {
            return WebClient;
        }
    }

    public class MockWebClient : ISimpleWebClient
    {
        private Dictionary<string, WebResponseEventArgs> _responses;

        public MockWebClient()
        {
            _responses = new Dictionary<string, WebResponseEventArgs>();
        }

        public void PrepareResponse(string uri, WebResponseEventArgs response)
        {
            _responses[uri] = response;
        }

        public void SendRequest(string uri, string method, string content)
        {
            WebResponseEventArgs response;
            if (!_responses.TryGetValue(uri.ToLowerInvariant(), out response))
                throw new Exception(string.Format("Uri not prepared {0}", uri));

            if (ResponseCompleted != null)
                ResponseCompleted(this, response);
        }

        public event EventHandler<WebResponseEventArgs> ResponseCompleted;
    }
}