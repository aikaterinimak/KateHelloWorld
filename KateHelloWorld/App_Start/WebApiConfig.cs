using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace KateHelloWorld
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    "GetGreetings",
            //    "api/{controller}/{cityid}/users/{userid}/greetings",
            //    new { controller = "Cities", action = "GetGreetings", cityid = "", userid = "" }
            //    );

            //config.Routes.MapHttpRoute(
            //    "PostGreetings",
            //    "api/{controller}/{cityid}/users/{userid}/greetings",
            //    new { controller = "Cities", action = "PostGreetings", cityid = "", userid = "" }
            //    );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
