﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CensusService
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Home", // Route name
				"", // URL with parameters
				new { controller = "Home", action = "Index", id = "" } // Parameter defaults
			);

			routes.MapRoute(
				"geo", // Route name
				"geo/{address}", // URL with parameters
				new { controller = "Search", action = "GeoCode", address = "" } // Parameter defaults
			);

			routes.MapRoute(
				"Census", // Route name
				"{pid}/{rid}/{dir}", // URL with parameters
				new { controller = "Search", action = "FamilySearch", pid = "113888907", rid = "6224", dir="prev" } // Parameter defaults
			);

			routes.MapRoute(
				"linkcensus", // Route name
				"recordlink/{pid}/{rid}/{familyId}/{compareFamilyId}", // URL with parameters
				new { controller = "Search", action = "RecordLink", familyId = "", compareFamilyLId = "" } // Parameter defaults
			);

			

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}
	}
}