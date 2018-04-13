using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace Setapp.Routes
{
    [ModuleDependency(typeof(InitializationModule))]
    public class RoutesInitializeModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RouteTable.Routes.MapRoute(
                "PerformanceTests",
                "performance-tests",
                new
                {
                    controller = "PerformanceTests",
                    action = "Index"
                }
            );

            RouteTable.Routes.MapRoute(
                "Home",
                "",
                new
                {
                    controller = "Home",
                    action = "Index"
                }
            );
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}