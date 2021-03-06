﻿using System.Web.Optimization;
using FluentValidation.Mvc;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Microposts.Models;
using Microposts.Migrations;

namespace Microposts
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FluentValidationModelValidatorProvider.Configure(provider =>
                                                                provider.AddImplicitRequiredValidator = false);
            Database.SetInitializer<ApplicationDbContext>(new TriggerInitializer());
        }
    }
}
