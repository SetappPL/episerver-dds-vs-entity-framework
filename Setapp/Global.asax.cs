using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Setapp
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
        }

        public override void Init()
        {
            base.Init();
            this.AcquireRequestState += ShowRouteValues;
        }

        protected void ShowRouteValues(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            if (context == null)
                return;
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));
        }
    }
}