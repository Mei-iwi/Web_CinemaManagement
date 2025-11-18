using System.Web.Mvc;

namespace Web_CinemaManagement.Areas.Manager
{
    public class ManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Manager_default",
                "Manager/{controller}/{action}/{id}",
                new { action = "Dashboard", id = UrlParameter.Optional },
                namespaces: new[] { "Web_CinemaManagement.Areas.Manager.Controllers" }
            );
        }
    }
}