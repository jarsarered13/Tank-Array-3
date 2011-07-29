using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using CensusService.Models;
using ServiceFramework;
using PersonModel;
using RecordLinkingFramework;
using Shared;
using RecordLinkage;
using System.IO;

namespace CensusService.Controllers
{
	[JsonpFilter]
	public class SearchController : Controller
	{
		private static string DefaultComparisonEngineConfigurationFileLocation = @"bin\DefaultComparisonEngine.xml";
			
		public SearchController()
		{
		}

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

			// Get the census year from database id
			int censusYear = CensusHelper.GetCensusYear(int.Parse(rid));
			
			// Get the previous/next census year
			int databaseId = CensusHelper.GetDatabaseId(censusYear, dir);

			// Get the previous database id
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

				simpleFamily.Id = family.Id;
				simpleFamily.CensusYear = searchCensusYear;
				//simpleFamily.Version = family.Version;
				
				simpleFamily.Mother = SimplePerson.CreatePerson(results.PersonContainer.GetPerson(family.MotherId), simpleFamily.CensusYear);
				simpleFamily.Father = SimplePerson.CreatePerson(results.PersonContainer.GetPerson(family.FatherId), simpleFamily.CensusYear);
				
				foreach (RelationshipPointer child in family.Children.ChildPointers)
				{
					simpleFamily.Children.Add(SimplePerson.CreatePerson(results.PersonContainer.GetPerson(child.Id), simpleFamily.CensusYear));
				}
				simpleFamily.FindPerson(results);
				familyResults.Add(simpleFamily);
			}		

			return new JsonResult
			{
				Data = familyResults,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};	
		}		
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pid"></param>
		/// <param name="rid"></param>
		/// <param name="familyId"></param>
		/// <param name="compareFamilyId"></param>
		/// <returns></returns>
		public ActionResult RecordLink(string pid, string rid, string familyId, string compareFamilyId)
		{
			DefaultComparisonEngineConfigurationFileLocation = Path.Combine(Request.PhysicalApplicationPath, DefaultComparisonEngineConfigurationFileLocation);
			NetworkClient client = new NetworkClient(new DataProperties());

			personRankConnectionPool = client.CreatePool(new ServerId[] { new ServerId("10.8.129.1:80") }, 60000, 60000);
			queryProcessor = new QueryProcessor(personRankConnectionPool);

			// TODO: Send person id
			int censusYear = CensusHelper.GetCensusYear(int.Parse(rid));
			int databaseId = CensusHelper.GetDatabaseId(censusYear, "prev");
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

		    Family family = results.PersonContainer.GetFamilies().Where(x => x.Id == familyId).FirstOrDefault();
			Family compareFamily = results.PersonContainer.GetFamilies().Where(x => x.Id == compareFamilyId).FirstOrDefault();

			// TODO: loop through the family
			IList<PersonModel.Person> personList = new List<PersonModel.Person>();
			IList<PersonModel.Person> personList2 = new List<PersonModel.Person>();
						
			foreach (RelationshipPointer child in family.Children.ChildPointers)
			{
				PersonModel.Person person = results.PersonContainer.GetPerson(child.Id);
				foreach (RelationshipPointer compareChild in compareFamily.Children.ChildPointers)
				{
					PersonModel.Person comparePerson = results.PersonContainer.GetPerson(child.Id);
					FeatureComparisonEngine comparisonEngine = new FeatureComparisonEngine(DefaultComparisonEngineConfigurationFileLocation);
					ComparisonResult result = comparisonEngine.ComparePeople(results.PersonContainer, person, comparePerson);
				}				
			}

			return new JsonResult
			{
				Data = string.Empty,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}
	}

	
}
