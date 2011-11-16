using Entile.Server;
using Nancy;
using Nancy.Conventions;
using TinyIoC;

namespace Entile.NancyHost
{
    public class ApiBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IMessageDispatcher>((c, n) => Bootstrapper.CurrentServer.CommandDispatcher);
            container.Register<IQueryDispatcher>((c, n) => Bootstrapper.CurrentServer.QueryDispatcher);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Scripts")
                );
        }
    }
}