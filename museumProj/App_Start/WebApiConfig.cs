using MultipartDataMediaFormatter;
using MultipartDataMediaFormatter.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace museumProj
{
    public static class WebApiConfig
    {
        //GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Formatters.Add
(new FormMultipartEncodedMediaTypeFormatter(new MultipartFormatterSettings()));
            // Web API configuration and services
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
               name: "defaultRpc",
               routeTemplate: "rpc/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional });
        }
    }
}
