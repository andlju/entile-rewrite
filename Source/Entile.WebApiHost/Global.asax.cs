using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Entile.Server;

namespace Entile.WebApiHost
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "RootApi",
                routeTemplate: "api",
                defaults: new { controller = "root" }
            );

            routes.MapHttpRoute(
                name: "ClientsApi",
                routeTemplate: "api/clients/{clientId}",
                defaults: new { clientId = RouteParameter.Optional, controller = "clients" }
            );

            routes.MapHttpRoute(
                name: "ClientSubscriptionsApi",
                routeTemplate: "api/clients/{clientId}/subscriptions/{subscriptionId}",
                defaults: new { subscriptionId = RouteParameter.Optional, controller = "clientsubscriptions" }
            );

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();
            BundleTable.Bundles.EnableBootstrapBundle();
        }

    }

    public static class BundleExtensions
    {
        public static void EnableBootstrapBundle(this BundleCollection bundles)
        {
            var bootstrapCss = new Bundle("~/Content/bootstrap/css", new CssMinify());
            bootstrapCss.AddFile("~/Content/bootstrap.css");
            bootstrapCss.AddFile("~/Content/bootstrap-responsive.css");
            bootstrapCss.AddFile("~/Content/application.css");

            bundles.Add(bootstrapCss);

            var bootstrapJs = new Bundle("~/bootstrap/js", new JsMinify());
            bootstrapJs.AddFile("~/Scripts/bootstrap.js");
            bootstrapJs.AddFile("~/Scripts/knockout.js");
            bundles.Add(bootstrapJs);
        }

    }
}