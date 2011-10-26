using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entile.Server;
using Entile.Server.Commands;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ModelBinding;
using Nancy.Responses;
using TinyIoC;

namespace Entile.NancyHost
{

    public class CommandModule : NancyModule
    {
        public CommandModule(IBus commandBus)
        {
            RegisterCommand<RegisterClientCommand>(commandBus);
            RegisterCommand<UnregisterClientCommand>(commandBus);
            RegisterCommand<RegisterSubscriptionCommand>(commandBus);
            RegisterCommand<UnregisterSubscriptionCommand>(commandBus);

            RegisterCommand<SendTileNotificationCommand>(commandBus);
            RegisterCommand<SendToastNotificationCommand>(commandBus);
            RegisterCommand<SendRawNotificationCommand>(commandBus);
        }

        private void RegisterCommand<TCommand>(IBus commandBus)
            where TCommand : ICommand, new()
        {
            var type = typeof (TCommand);

            Get[type.Name] = _ =>
                                 {
                                     var fields =
                                         type.GetFields().Select(f => new {f.Name, Type = f.FieldType.Name}).ToArray();
                                     
                                     return new JsonResponse(new { Method = "POST", Fields = fields });
                                 };

            Post[type.Name] = _ =>
                                  {
                                      TCommand cmd = this.Bind();
                                      commandBus.Publish(cmd);
                                      return new JsonResponse(new {Status = "accepted"});
                                  };

        }
    }
    
    public class CommandBootstrapper : DefaultNancyBootstrapper
    {
        protected override void InitialiseInternal(TinyIoCContainer container)
        {
            container.Register<IBus>((c, n) => Bootstrapper.CurrentServer.CommandBus);
        }
    }
}