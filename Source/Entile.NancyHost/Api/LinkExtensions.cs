using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Entile.Server;

namespace Entile.NancyHost.Api
{
    public static class LinkExtensions
    {
        public static IEnumerable<object> ToLinks(this Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return methods.ToLinks();
        }

        public static IEnumerable<object> ToLinks(this IEnumerable<MethodInfo> methodInfos)
        {
            return methodInfos.Select(method => ToLink(method)).ToArray();
        }

        public static IEnumerable<object> ToRootLink(this Type type)
        {
            var method = type.GetMethod("Self");
            return new[] { method.ToLink() };
        } 

        private static object ToLink(this MethodInfo method)
        {
            return new
                       {
                           Uri = method.GetUri(),
                           Rel = method.GetRel()
                       };
        }

        public static string GetResourceName(this Type type)
        {
            var apiAttrib = (ApiAttribute)type.GetCustomAttributes(typeof(ApiAttribute), true).FirstOrDefault();

            string resourceName = type.Name.Replace("ApiModel", string.Empty);
            if (apiAttrib != null)
            {
                resourceName = apiAttrib.ResourceName ?? resourceName;
            }

            return resourceName;
        }

        public static string GetRel(this MethodInfo method)
        {
            var apiMethodAttrib = (ApiMethodAttribute)method.GetCustomAttributes(typeof(ApiMethodAttribute), true).FirstOrDefault();

            string rel = method.Name == "Self" ? method.DeclaringType.GetResourceName() : method.Name;

            if (apiMethodAttrib != null)
            {
                rel = apiMethodAttrib.Rel ?? rel;
            }
            return rel;
        }

        public static string GetBaseUri(this Type type)
        {
            var apiAttrib = (ApiAttribute)type.GetCustomAttributes(typeof (ApiAttribute), true).FirstOrDefault();
            if (apiAttrib == null)
            {
                return "/" + type.GetResourceName();
            }
            return apiAttrib.BaseUri;
        }

        public static string GetUri(this MethodInfo method)
        {
            var apiMethodAttrib = (ApiMethodAttribute)method.GetCustomAttributes(typeof(ApiMethodAttribute), true).FirstOrDefault();

            string baseUri = GetBaseUri(method.DeclaringType);
            string relativeUri = method.Name == "Self" ? "" : "/" + method.Name;

            if (apiMethodAttrib != null)
            {
                relativeUri = apiMethodAttrib.RelativeUri ?? relativeUri;
            }
            return baseUri + relativeUri;
        }

        public static string GetHttpMethod(this MethodInfo method)
        {
            var apiMethodAttrib = (ApiMethodAttribute)method.GetCustomAttributes(typeof(ApiMethodAttribute), true).FirstOrDefault();

            string httpMethod = method.Name == "Self" ? "GET" : "POST";

            if (apiMethodAttrib != null)
            {
                httpMethod = apiMethodAttrib.HttpMethod ?? httpMethod;
            }
            return httpMethod;
        }

        public static object AsForm(this MethodInfo method)
        {
            var httpMethod = method.GetHttpMethod();
            var messageProperties = GetMessageType(method).GetFields();

            return new
                       {
                           Action = httpMethod,
                           Form = messageProperties.ToDictionary(pi => pi.Name, _ => (object)null)
                       };
        }

        public static Type GetMessageType(this MethodInfo method)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != 1 || !typeof(IMessage).IsAssignableFrom(parameters[0].ParameterType))
                return null;

            var messageType = parameters[0].ParameterType;
            return messageType;
        }
    }
}