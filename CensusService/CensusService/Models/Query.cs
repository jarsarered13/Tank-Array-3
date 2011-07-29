using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceFramework;
using System.Xml;
using PersonModel;
using Shared;

namespace CensusService.Models
{
	public class QueryResultList : List<QueryResult>
	{
		public PersonContainer PersonContainer;

		public QueryResultList(PersonContainer personContainer)
		{
			this.PersonContainer = personContainer;
		}
	}




	public class QueryResult
	{
		public Person Person;
		public int Score;
		public int Quality;

		public QueryResult(XmlElement queryResultNode, PersonContainer personContainer)
		{
			PersonId personId = PersonId.Create(XmlUtil.GetAttributeValue(queryResultNode, "id"));

			this.Person = personContainer.GetPerson(personId);
			this.Score = XmlUtil.GetAttributeInt(queryResultNode, "s");
			this.Quality = XmlUtil.GetAttributeInt(queryResultNode, "q");
		}


		public QueryResult(PersonId personId, int score, int quality, PersonContainer personContainer)
		{
			this.Person = personContainer.GetPerson(personId);
			this.Score = score;
			this.Quality = quality;
		}
	}
	
	public class QueryProcessor
	{
		private ICommunicationPool connectionPool;

		public QueryProcessor(ICommunicationPool connectionPool)
		{
			this.connectionPool = connectionPool;
		}

		public QueryResultList ExecuteQuery(string personId)
		{

			CommunicationMessage request = new CommunicationMessage(MessageType.Request);
			request.Command = "Query";
			request.Add("personId", personId);
			request.Add("maxResults", "20");
			request.Add("minComparisonScore", "1");
			request.Add("depth", "1");
			request.Add("collectionNames", "Census");

			CommunicationMessage response = connectionPool.Send(request);
			if (response.HasError) throw new Exception(response.Error);

			XmlNode personCacheNode = response.Xml.SelectSingleNode("personCache");
			PersonContainer container = Serializer.Container.Read(personCacheNode.OuterXml);

			QueryResultList queryResultList = new QueryResultList(container);

			XmlNodeList matches = response.Xml.SelectNodes("match");
			foreach (XmlElement match in matches)
			{
				queryResultList.Add(new QueryResult(match, container));
			}

			return queryResultList;
		}
	}
}