using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using Entile.Server.Commands;

namespace Entile.WebApiHost.Controllers
{

    public abstract class HyperMediaProviderBase<T> where T : HyperMediaResponse
    {
        private static Regex _paramRegex = new Regex(@"\{(?<param>\w+)\}", RegexOptions.Compiled);

        public void AddHyperMedia(HttpRequestMessage request, T response)
        {
            var links = Links(request, response);
            var commands = Commands(request, response);

            if (links != null)
                response.Links = new List<LinkDefinition>(links);

            if (commands != null)
                response.Commands = new List<CommandDefinition>(commands);
        }

        protected virtual IEnumerable<LinkDefinition> Links(HttpRequestMessage request, T response)
        {
            return null;
        }

        protected virtual IEnumerable<CommandDefinition> Commands(HttpRequestMessage request, T response)
        {
            return null;
        }
        
        protected Uri BuildUri(HttpRequestMessage request, string relativeUri)
        {
            return new Uri(request.RequestUri, relativeUri);
        }

        protected CommandDefinition BuildCommand(HttpRequestMessage request, T response, string uriTemplate, string name, string description, string method = null)
        {
            var relativeUri = FillTemplate(uriTemplate, response);
            var uri = BuildUri(request, relativeUri);
            var def = new CommandDefinition()
                          {
                              Name = name,
                              Description = description,
                              Method = method ?? "POST",
                              Uri = uri
                          };
            return def;
        }

        protected CommandDefinition BuildCommand<TCmd>(HttpRequestMessage request, T response, string uriTemplate, string name, string description, string method = null) where TCmd : ICommand
        {
            var def = BuildCommand(request, response, uriTemplate, name, description, method);

            var props = typeof(TCmd).GetProperties();
            def.Fields = props.Where(p => !GetKey(p)).Select(p => new FieldDefinition()
                                               {
                                                   Name = p.Name, 
                                                   Description = GetDescription(p), 
                                                   Optional = !GetRequired(p)
                                               }).ToArray();
            return def;
        }

        private string GetDescription(PropertyInfo propertyInfo)
        {
            var displayAttr = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
            if (displayAttr == null)
                return propertyInfo.Name;
            return displayAttr.Description ?? propertyInfo.Name;
        }

        private bool GetRequired(PropertyInfo propertyInfo)
        {
            var requiredAttr = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;
            if (requiredAttr == null)
                return false;
            return true;
        }

        private bool GetKey(PropertyInfo propertyInfo)
        {
            var requiredAttr = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault() as KeyAttribute;
            if (requiredAttr == null)
                return false;
            return true;
        }

        protected LinkDefinition BuildLink(HttpRequestMessage request, T response, string uriTemplate, string rel)
        {
            var relativeUri = FillTemplate(uriTemplate, response);

            return new LinkDefinition() { Rel = rel, Uri = BuildUri(request, relativeUri) };
        }

        private string FillTemplate(string uriTemplate, object context)
        {
            var uri = _paramRegex.Replace(uriTemplate, m => GetParameter(m, context));
            return uri;
        }

        private string GetParameter(Match m, object context)
        {
            if (context == null)
                return m.Value;

            var paramName = m.Groups["param"].Value;
            var contextType = context.GetType();

            var prop = contextType.GetProperty(paramName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (prop != null)
                return prop.GetValue(context, null).ToString();

            return m.Value;
        }

    }
}