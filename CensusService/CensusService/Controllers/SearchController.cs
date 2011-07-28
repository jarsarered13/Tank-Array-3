using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using CensusService.Models;
using ServiceFramework;
using PersonModel;
using Shared;

namespace CensusService.Controllers
{
	[JsonpFilter]
	public class SearchController : Controller
	{
		private ICommunicationPool personRankConnectionPool;
		private QueryProcessor queryProcessor;

		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pid"></param>
		/// <param name="rid"></param>
		/// <returns></returns>
		public ActionResult FamilySearch(string pid, string rid, string dir)
		{
			NetworkClient client = new NetworkClient(new DataProperties());

			personRankConnectionPool = client.CreatePool(new ServerId[] { new ServerId("10.8.129.1:80") }, 60000, 60000);
			queryProcessor = new QueryProcessor(personRankConnectionPool);

			//string startingId = "6224";
			int censusYear = CensusHelper.GetCensusYear(int.Parse(rid));
			
			int databaseId = CensusHelper.GetDatabaseId(censusYear, dir);

			int searchCensusYear = CensusHelper.GetCensusYear(databaseId);

			QueryResultList results;
			try
			{
				results = queryProcessor.ExecuteQuery(string.Format("{0}:{1}", pid, rid));				
			}
			catch
			{
				return new JsonResult
				{
					Data = string.Empty,
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};
			}


			List<SimpleFamily> familyResults = new List<SimpleFamily>();
			List<Family> filteredResults = null;

			filteredResults = results.PersonContainer.GetFamilies().Where(x => x.Id.Contains(databaseId.ToString())).Take(5).ToList();
			foreach (Family family in filteredResults)
			{
				SimpleFamily simpleFamily = new SimpleFamily();
				simpleFamily.CensusYear = searchCensusYear;
				simpleFamily.Version = family.Version;
				simpleFamily.SearchPersonId = pid;
				simpleFamily.Mother = new SimplePerson(results.PersonContainer.GetPerson(family.MotherId), simpleFamily.CensusYear);
				simpleFamily.Father = new SimplePerson(results.PersonContainer.GetPerson(family.FatherId), simpleFamily.CensusYear);


				foreach (RelationshipPointer child in family.Children.ChildPointers)
				{
					simpleFamily.Children.Add(new SimplePerson(results.PersonContainer.GetPerson(child.Id), simpleFamily.CensusYear));
				}

				familyResults.Add(simpleFamily);
			}

			//foreach (QueryResult result in results)
			//{
			//    SimpleFamily family = new SimpleFamily();				
			//    family.SearchPersonId = pid;
				
				
			//    //family

			//    if (result.Person.Parents.Count > 0)
			//    {
			//        foreach (Family parentFamily in result.Person.Parents)
			//        {
			//            family.Version = parentFamily.Version;						
			//            family.SearchPersonId = pid;
			//            family.Mother = new SimplePerson(results.PersonContainer.GetPerson(parentFamily.MotherId));
			//            family.Father = new SimplePerson(results.PersonContainer.GetPerson(parentFamily.FatherId));

			//            foreach (RelationshipPointer child in parentFamily.Children)
			//            {
			//                family.Children.Add(new SimplePerson(results.PersonContainer.GetPerson(child.Id)));
			//            }						
			//        }
			//    }

			//    familyResults.Add(family);
			//}

			return new JsonResult
			{
				Data = familyResults,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet

			};

			//return Json(familyResults, JsonRequestBehavior.AllowGet);
			//StringBuilder htmlBuffer = new StringBuilder();



			//foreach (QueryResult result in results)
			//{
			//    //htmlBuffer.AppendFormat("<b>{0}</b> ({1}) - <span style='color: silver;'>{2} {3}</span><br/>", result.Person.Name, result.Person.Id, result.Score, result.Quality);
			//    if (result.Person.Parents.Count > 0)
			//    {
			//        foreach (Family parentFamily in result.Person.Parents)
			//        {
			//            Person mother = results.PersonContainer.GetPerson(parentFamily.MotherId);
			//            Person father = results.PersonContainer.GetPerson(parentFamily.FatherId);

			//            htmlBuffer.AppendFormat("Mom: {0}, Dad: {1}<br/>", mother.Name, father.Name);

			//            foreach (RelationshipPointer child in parentFamily.Children)
			//            {
			//                Person sibling = results.PersonContainer.GetPerson(child.Id);
			//                htmlBuffer.AppendFormat("Sibling: {0}<br/>", sibling.Name);
			//            }
			//        }
			//    }
			//    if (result.Person.Marriages.Count > 0)
			//    {
			//        htmlBuffer.AppendFormat("Marriage count is greater than 0.<br/>");
			//    }
			//}
		}
	}
}
