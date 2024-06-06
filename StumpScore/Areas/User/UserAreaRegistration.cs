using System.Web.Mvc;

namespace StumpScore.Areas.User
{
    public class UserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "User_default",
                "User/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
        //public override void RegisterArea(AreaRegistrationContext context)
        //{
        //    context.MapRoute(
        //        "User_default",
        //        "{Area}/{controller}/{action}/{id}",
        //        new { area = "User", controller = "Home", action = "Index", id = UrlParameter.Optional }
        //    );
        //}
    }
}