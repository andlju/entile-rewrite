using System;

namespace Entile.NancyHost.Api
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    internal sealed class ApiAttribute : Attribute
    {
        // This is a positional argument
        public ApiAttribute(string baseUri)
        {
            BaseUri = baseUri;
        }

        public string BaseUri { get; private set; }
        public string ResourceName { get; set; }

    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    internal sealed class ApiMethodAttribute : Attribute
    {
        public bool Entrypoint { get; set; }

        public string RelativeUri { get; set; }

        public string HttpMethod { get; set; }

        public string Rel { get; set; }

    }
    
}