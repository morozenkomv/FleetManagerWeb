namespace FleetManagerWeb
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional });

            routes.MapRoute(name: "Logout", url: "home/logout",
                defaults: new { controller = "Home", action = "Logout" });

            routes.MapRoute(name: "PermissionRedirectPage", url: "home/PermissionRedirectPage",
                defaults: new { controller = "Home", action = "PermissionRedirectPage" });            
        }
    }
}