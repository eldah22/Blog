using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SABlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Update-Profile",
                url: "updateProfile",
                defaults: new { controller = "User", action = "Update" }
            );


            routes.MapRoute(
                name: "Profile",
                url: "u/{username}",
                defaults: new { controller = "User", action = "UserProfile", username = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "Create-Post",
                url: "createPost",
                defaults: new { controller = "Post", action = "CreatePost" }
            );

            routes.MapRoute(
                name: "Post",
                url: "u/{username}/{postId}",
                defaults: new { controller = "Post", action = "Index", username = UrlParameter.Optional, postId = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "Create-Category",
                url: "createCategory",
                defaults: new {controller = "Category", action = "Create"}
            );
          

            routes.MapRoute(
                name: "Category",
                url: "category/{categoryName}",
                defaults: new { controller = "Category", action = "Details", childCategory = UrlParameter.Optional }
            );

           

            routes.MapRoute(
                name: "Search",
                url: "search",
                defaults: new { controller = "Home", action = "Search"}
            );

            routes.MapRoute(
                name: "PrivacyPolicty",
                url: "privacy",
                defaults: new { controller = "Home", action = "PrivacyPolicy" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
          

        }
    }
}
