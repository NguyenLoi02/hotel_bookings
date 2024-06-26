﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace hotel_bookings
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hotel_bookings.Controllers" }
            );

            routes.MapRoute(
              name: "Area_default",
              url: "{area}/{controller}/{action}/{id}",
              defaults: new { area = "Admin", controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "hotel_bookings.Areas.Admin.Controllers" }
          );
           
        }
    }
}
