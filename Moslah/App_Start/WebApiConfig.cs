using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Moslah
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            XmlMediaTypeFormatter xmlFormat = config.Formatters.XmlFormatter;
            config.Formatters.Remove(xmlFormat);

            JsonMediaTypeFormatter jformat = config.Formatters.JsonFormatter;
            jformat.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jformat.SerializerSettings.DateFormatString = "dd/MM/yyyy";

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
