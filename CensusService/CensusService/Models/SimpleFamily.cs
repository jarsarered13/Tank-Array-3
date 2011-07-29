﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonModel;
using System.Drawing;
using Shared;

namespace CensusService.Models
{
	public class SimpleFamily
	{
		public class Location
		{
			public long X {get;set;}
			public long Y { get; set; }
		}

		private List<SimplePerson> _children = null;
				
		public string Id { get; set; }
		public int CensusYear { get; set; }
		public string SelectedPersonId { get; set; }
		public SimplePerson Mother {get;set;}
		public SimplePerson Father { get; set; }
		public List<SimplePerson> Children 
		{ 
			get
			{
				if (_children == null)
				{
					_children = new List<SimplePerson>();
				}

				return _children;
			} 
			set
			{
				_children = value;
			} 
		}
		public string Address { get; set; }

		public void FindPerson(QueryResultList results)
		{
			QueryResult r = results.Where(x => Id.Contains(x.Person.Id.DatabaseId.ToString()))
				.FirstOrDefault();

			if (r != null)
			{				
				Address = r.Person.GeneralEvents.Best.Place;
			}

			foreach (QueryResult result in results)
			{
				if (Father.FullName == result.Person.Name)
				{
					SelectedPersonId = result.Person.Id.Pid.ToString();
					Father.Selected = true;
					return;
				}

				if (Mother.FullName == result.Person.Name)
				{
					SelectedPersonId = result.Person.Id.Pid.ToString();
					Mother.Selected = true;
					return;
				}

				foreach (SimplePerson person in Children)
				{
					if (result.Person.Name == person.FullName)
					{
						SelectedPersonId = result.Person.Id.Pid.ToString();
						person.Selected = true;
						return;
					}

				}	
			}
		}
	}
}