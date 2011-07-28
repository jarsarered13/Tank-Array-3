using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

using ServiceFramework;


namespace CensusService.Controllers
{
	public class HomeController : Controller
	{

		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}
	}
}
