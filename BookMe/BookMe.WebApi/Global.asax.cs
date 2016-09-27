﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BookMe.Infrastructure;
using BookMe.Infrastructure.IoC;
using BookMe.Infrastructure.MapperConfiguration;

namespace BookMe.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            IoCConfig.RegisterControllers(GlobalConfiguration.Configuration);
            AutoMapperConfiguration.Configure();
            GlobalConfiguration.Configuration.MessageHandlers.Add(new XHttpMethodDelegatingHandler());
        }
    }
}