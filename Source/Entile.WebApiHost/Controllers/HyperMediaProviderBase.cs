using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using Entile.Server;
using Entile.Server.Commands;
using Entile.WebApiHost.Models;

namespace Entile.WebApiHost.Controllers
{

    public abstract class HyperMediaProviderBase<T> where T : HyperMediaResponse
    {
        private static Regex _paramRegex = new Regex(@"\{(?<param>\w+)\}", RegexOptions.Compiled);

        protected class CommandBuilder
        {
            public string UriTemplate;
            public string Name;
            public string Description;
            public string Method;
            public Type CommandType;
        }

        protected class QueryBuilder
        {
            public string UriTemplate;
            public string Name;
            public string Description;
            public Type QueryType;
        }

        protected class LinkBuilder
        {
            public string UriTemplate;
            public string Name;
        }

        public void AddHyperMedia(HttpRequestMessage request, T response)
        {
            var links = Links(response);
            var commands = Commands(response);
            var queries = Queries(response);

            if (links != null)
                response.Links = links.Select(l => BuildLink(request, response, l.UriTemplate, l.Name)).ToList();
            if (queries != null)
                response.Queries = queries.Select(q => BuildQuery(request, response, q.UriTemplate, q.Name, q.Description, q.QueryType)).ToList();
            if (commands != null)
                response.Commands = commands.Select(c => BuildCommand(request, response, c.UriTemplate, c.Name, c.Description, c.CommandType, c.Method)).ToList();
        }

        public LinkDefinition GetLink(string name, HttpRequestMessage request, T response)
        {
            var link = Links(response).FirstOrDefault(l => l.Name == name);
            if (link == null)
                return null;

            return BuildLink(request, response, link.UriTemplate, link.Name); ;
        }

        protected virtual IEnumerable<LinkBuilder> Links(T response)
        {
            return null;
        }

        protected virtual IEnumerable<QueryBuilder> Queries(T response)
        {
            return null;
        }

        protected virtual IEnumerable<CommandBuilder> Commands(T response)
        {
            return null;
        }
        
        protected static LinkBuilder Link(string uriTemplate, string name)
        {
            return new LinkBuilder() { Name = name, UriTemplate = uriTemplate};
        }

        protected static CommandBuilder Command(string uriTemplate, string name, string description, string method = null)
        {
            return new CommandBuilder()
            {
                UriTemplate = uriTemplate,
                Name = name,
                Description = description,
                Method = method ?? "POST"
            };
        }

        protected static QueryBuilder Query<TQuery>(string uriTemplate, string name, string description) where TQuery : IMessage
        {
            return new QueryBuilder()
            {
                UriTemplate = uriTemplate,
                Name = name,
                Description = description,
                QueryType = typeof(TQuery)
            };
        }

        protected static CommandBuilder Command<TCmd>(string uriTemplate, string name, string description, string method = null) where TCmd : ICommand
        {
            return new CommandBuilder()
            {
                UriTemplate = uriTemplate,
                Name = name,
                Description = description,
                Method = method ?? "POST",
                CommandType = typeof(TCmd)
            };
        }

        private static Uri BuildUri(HttpRequestMessage request, string relativeUri)
        {
            return new Uri(request.RequestUri, relativeUri);
        }

        private static QueryDefinition BuildQuery(HttpRequestMessage request, T response, string uriTemplate, string name, string description, Type queryType)
        {
            var relativeUri = FillTemplate(uriTemplate, response);
            var uri = BuildUri(request, relativeUri);
            var def = new QueryDefinition()
            {
                Name = name,
                Description = description,
                Uri = uri
            };
            if (queryType != null)
            {
                var props = queryType.GetProperties();

                def.Fields = props.Where(p => !GetKey(p)).Select(p => new FieldDefinition()
                {
                    Name = p.Name,
                    Description = GetDescription(p),
                    Optional = !GetRequired(p)
                }).ToArray();
            }
            return def;
        }

        private static CommandDefinition BuildCommand(HttpRequestMessage request, T response, string uriTemplate, string name, string description, Type cmdType, string method)
        {
            var relativeUri = FillTemplate(uriTemplate, response);
            var uri = BuildUri(request, relativeUri);
            var def = new CommandDefinition()
                          {
                              Name = name,
                              Description = description,
                              Method = method,
                              Uri = uri
                          };
            if (cmdType != null)
            {
                var props = cmdType.GetProperties();

                def.Fields = props.Where(p => !GetKey(p)).Select(p => new FieldDefinition()
                                                                          {
                                                                              Name = p.Name,
                                                                              Description = GetDescription(p),
                                                                              Optional = !GetRequired(p)
                                                                          }).ToArray();
            }
            return def;
        }

        private static string GetDescription(PropertyInfo propertyInfo)
        {
            var displayAttr = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
            if (displayAttr == null)
                return propertyInfo.Name;
            return displayAttr.Description ?? propertyInfo.Name;
        }

        private static bool GetRequired(PropertyInfo propertyInfo)
        {
            var requiredAttr = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;
            if (requiredAttr == null)
                return false;
            return true;
        }

        private static bool GetKey(PropertyInfo propertyInfo)
        {
            var requiredAttr = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault() as KeyAttribute;
            if (requiredAttr == null)
                return false;
            return true;
        }

        protected static LinkDefinition BuildLink(HttpRequestMessage request, T response, string uriTemplate, string rel)
        {
            var relativeUri = FillTemplate(uriTemplate, response);

            return new LinkDefinition() { Name = rel, Uri = BuildUri(request, relativeUri) };
        }

        private static string FillTemplate(string uriTemplate, object context)
        {
            var uri = _paramRegex.Replace(uriTemplate, m => GetParameter(m, context));
            return uri;
        }

        private static string GetParameter(Match m, object context)
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