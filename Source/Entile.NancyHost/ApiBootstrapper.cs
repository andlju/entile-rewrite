using Entile.Server;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using TinyIoC;

namespace Entile.NancyHost
{
    public class ApiBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IMessageDispatcher>((c, n) => Bootstrapper.CurrentServer.CommandDispatcher);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            
            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Scripts")
                );
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                // Don't want any special handling for errors
                return NancyInternalConfiguration.WithOverrides(cfg => cfg.ErrorHandlers.Clear());
            }
        }
    }
}