using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entile.NancyHost.Api;
using Entile.Server;
using Nancy;
using Nancy.ModelBinding;
using TinyIoC;

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

                Func<object, Response> invokeAction = _ =>
                                                          {
                                                              var model = container.Resolve(modelType);
                                                              object result;
                                                              if (messageType != null)
                                                              {
                                                                  var message = GetMessage(messageType);
                                                                  result = currentMethod.Invoke(model, new[] { message });
                                                              } 
                                                              else
                                                              {
                                                                  result = currentMethod.Invoke(model, null);
                                                              }

                                                              return Response.AsJson(result);
                                                          };

                var httpMethod = currentMethod.GetHttpMethod();
                var uri = currentMethod.GetUri();

                if (httpMethod == "GET")
                {
                    Get[modelType.GetBaseUri()] = invokeAction;
                } 
                else
                {
                    Get[uri] = _ => Response.AsJson(currentMethod.AsForm());

                    if (httpMethod == "POST")
                    {
                        Post[uri] = invokeAction;
                    }
                    else if (httpMethod == "PUT")
                    {
                        Put[uri] = invokeAction;
                    }
                    else if (httpMethod == "DELETE")
                    {
                        Delete[uri] = invokeAction;
                    }
                }
            }
        }

        private IMessage GetMessage(Type commandType)
        {
            var binder = ModelBinderLocator.GetBinderForType(commandType);
            var command = binder.Bind(Context, commandType) as IMessage;
            foreach (var param in Context.Parameters)
            {
                FieldInfo field = commandType.GetField(param,
                                                       BindingFlags.Public | 
                                                       BindingFlags.Instance | 
                                                       BindingFlags.IgnoreCase);
                if (field != null)
                {
                    var convertMethod =
                        ConvertMethod.MakeGenericMethod(
                            field.FieldType);
                    try
                    {
                        var converted = convertMethod.Invoke(
                            null, new[] { Context.Parameters[param] }
                            );
                        field.SetValue(command, converted);
                    }
                    catch (TargetInvocationException ex)
                    {
                        
                    }
                }
            }
            return command;
        }

        static T Convert<T>(dynamic d)
        {
            return (T) d;
        }

        private static MethodInfo ConvertMethod = typeof (ApiModule).GetMethod("Convert", BindingFlags.Static | BindingFlags.NonPublic);
    }

}