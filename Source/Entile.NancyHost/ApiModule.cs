using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entile.NancyHost.Api;
using Entile.Server;
using Nancy;
using Nancy.ModelBinding;
using TinyIoC;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace Entile.NancyHost
{
    public class ApiModule : NancyModule
    {

        public ApiModule(TinyIoCContainer container)
        {
            RegisterModelType(container, typeof(RootApiModel));
            RegisterModelType(container, typeof(ClientApiModel));
            RegisterModelType(container, typeof(SubscriptionApiModel));
            
            Get["/"] = _ => View["Views/ApiBrowser.html"];

        }

        private void RegisterModelType(TinyIoCContainer container, Type modelType)
        {
            var methods = modelType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var method in methods)
            {
                var currentMethod = method;

                var messageType = method.GetMessageType();
                
                var httpMethod = currentMethod.GetHttpMethod();
                var uri = currentMethod.GetUri();

                if (httpMethod == "GET")
                {
                    Get[uri] = _ => InvokeModelAction(container, modelType, currentMethod, messageType);
                } 
                else
                {
                    Get[uri] = _ => Response.AsJson(ToForm(currentMethod));

                    if (httpMethod == "POST")
                    {
                        Post[uri] = _ => InvokeModelAction(container, modelType, currentMethod, messageType);
                    }
                    else if (httpMethod == "PUT")
                    {
                        Put[uri] = _ => InvokeModelAction(container, modelType, currentMethod, messageType);
                    }
                    else if (httpMethod == "DELETE")
                    {
                        Delete[uri] = _ => InvokeModelAction(container, modelType, currentMethod, messageType);
                    }
                }
            }
        }

        private Response InvokeModelAction(TinyIoCContainer container, Type modelType, MethodInfo currentMethod, Type messageType)
        {
            try
            {
                var model = (ApiModelBase)container.Resolve(modelType);
                object result;
                if (messageType != null)
                {
                    var message = GetMessage(messageType);
                    result = currentMethod.Invoke(model, new[] {message});
                }
                else
                {
                    result = currentMethod.Invoke(model, null);
                }
                
                var response = Response.AsJson(result);
                response.StatusCode = (HttpStatusCode)model.HttpStatusCode;

                return response;
            }
            catch (Exception ex)
            {
                var response = Response.AsJson(new {ex.Message});
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }
        }

        private IMessage GetMessage(Type commandType)
        {
            var binder = ModelBinderLocator.GetBinderForType(commandType);
            var command = binder.Bind(Context, commandType) as IMessage;
            foreach (var param in Context.Parameters)
            {
                FieldInfo field = commandType.GetField(param, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (field != null)
                {
                    var convertMethod = ConvertMethod.MakeGenericMethod(field.FieldType);
                    try
                    {
                        var converted = convertMethod.Invoke(null, new[] { Context.Parameters[param] });

                        field.SetValue(command, converted);
                    }
                    catch (TargetInvocationException)
                    {
                        
                    }
                }
            }
            return command;
        }

        private object ToForm(MethodInfo method)
        {
            var httpMethod = method.GetHttpMethod();
            var messageProperties = method.GetMessageType().GetFields();

            return new
            {
                Action = httpMethod,
                Form = messageProperties.ToDictionary(pi => pi.Name, _ => (object)null)
            };
        }

        static T Convert<T>(dynamic d)
        {
            return (T) d;
        }

        private static MethodInfo ConvertMethod = typeof(ApiModule).GetMethod("Convert", BindingFlags.Static | BindingFlags.NonPublic);
    }

}