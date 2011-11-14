using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Entile.NancyHost.Api;
using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Queries;
using Entile.Server.ViewHandlers;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.ModelBinding;
using Nancy.Responses;
using TinyIoC;

namespace Entile.NancyHost
{
    public class CommandBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IMessageDispatcher>((c, n) => Bootstrapper.CurrentServer.CommandDispatcher);
            container.Register<IQueryDispatcher>((c, n) => Bootstrapper.CurrentServer.QueryDispatcher);
        }

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Scripts")
                );
        }
    }
}