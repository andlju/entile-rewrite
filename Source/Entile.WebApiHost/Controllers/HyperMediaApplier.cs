using System;
using System.Collections.Generic;
using System.Net.Http;
using Entile.WebApiHost.Models;

namespace Entile.WebApiHost.Controllers
{
    public class HyperMediaApplier
    {
        private readonly Dictionary<Type, object> _providers = new Dictionary<Type, object>();

        public void RegisterProvider<T>(HyperMediaProviderBase<T> provider) where T : HyperMediaResponse
        {
            _providers[typeof (T)] = provider;
        }

        public Uri GetLink<TResponse>(string linkName, HttpRequestMessage request, TResponse response) where TResponse : HyperMediaResponse
        {
            object obj;
            if (!_providers.TryGetValue(typeof(TResponse), out obj))
                return null;
            var provider = (HyperMediaProviderBase<TResponse>)obj;
            
            return provider.GetLink(linkName, request, response).Uri;
        }

        public void Apply<TResponse>(HttpRequestMessage request, TResponse response) where TResponse : HyperMediaResponse
        {
            object obj;
            if (!_providers.TryGetValue(typeof(TResponse), out obj))
                return;
            var provider = (HyperMediaProviderBase<TResponse>) obj;
            provider.AddHyperMedia(request, response);
        }
    }
}