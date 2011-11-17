using System;
using System.Reflection;
using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Queries;
using Nancy;

namespace Entile.NancyHost
{
    /*
    public class ClientModule : NancyModule
    {
         public ClientModule(IMessageDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
         {
             Get["/client/{clientId}"] = _ =>
                                             {
                                                 var query = GetMessage<GetClientQuery>();
                                                 dynamic response = queryDispatcher.Invoke(query);

                                                 string clientBaseUri = "/client/" + query.ClientId;

                                                 foreach(dynamic sub in response.Subscriptions)
                                                 {
                                                     sub.Links = new[]
                                                                     {
                                                                         new { Uri = clientBaseUri + "/subscription/" + sub.SubscriptionId, Rel = "subscription"}
                                                                     };
                                                 }
                                                 response.Links = new[]
                                                                       {
                                                                           new { Uri = clientBaseUri, Rel = "self" },
                                                                           new { Uri = clientBaseUri + "/register", Rel = "register" },
                                                                           new { Uri = clientBaseUri + "/unregister", Rel = "unregister" },
                                                                       };
                                                 return Response.AsJson((object)response);
                                             };

             Put["/client/{clientId}/register"] = _ =>
                                             {
                                                 var command = GetMessage<RegisterClientCommand>();
                                                 commandDispatcher.Dispatch(command);

                                                 return Response.AsJson(
                                                     new { Links = new[]
                                                                       {
                                                                           new { Uri = "/client/" + command.ClientId, Rel = "self" },
                                                                           new { Uri = "/client/" + command.ClientId + "/register", Rel = "register" },
                                                                           new { Uri = "/client/" + command.ClientId + "/unregister", Rel = "unregister" },
                                                                       }
                                                     });
                                             };
         }

         private T GetMessage<T>() where T : class, IMessage
         {
             var commandType = typeof(T);
             var binder = ModelBinderLocator.GetBinderForType(commandType);
             var command = binder.Bind(Context, commandType) as T;
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
             return (T)d;
         }

         private static MethodInfo ConvertMethod = typeof(ApiModule).GetMethod("Convert", BindingFlags.Static | BindingFlags.NonPublic);

    }*/
}