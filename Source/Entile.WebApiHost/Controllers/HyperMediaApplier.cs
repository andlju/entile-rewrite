using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Entile.WebApiHost.Controllers
{
    public class HyperMediaApplier
    {
        private Dictionary<Type, object> _providers = new Dictionary<Type, object>();

        public void RegisterProvider<T>(HyperMediaProviderBase<T> provider) where T : HyperMediaResponse
        {
            _providers[typeof (T)] = provider;
        }

        public void Apply(HttpRequestMessage request, HyperMediaResponse response)
        {
            object obj;
            if (!_providers.TryGetValue(response.GetType(), out obj))
                return;
            var method = obj.GetType().GetMethod("AddHyperMedia", new Type[] {typeof(HttpRequestMessage), response.GetType()});
            method.Invoke(obj, new object[] { request, response });
        }
    }
}