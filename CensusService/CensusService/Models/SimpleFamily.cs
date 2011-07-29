using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonModel;
using System.Drawing;
using Shared;
using Augmentation.Authorities.PlaceAuthority;

namespace CensusService.Models
{
	public class SimpleFamily
	{
		public class Location
		{
			public double X {get;set;}
			public double Y { get; set; }
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
		public Location GeoLoc { get; set; }
		public string Address { get; set; }

		public void FindPerson(QueryResultList results)
		{
			QueryResult r = results.Where(x => Id.Contains(x.Person.Id.DatabaseId.ToString()))
				.FirstOrDefault();

			if (r != null)
			{			
				// TODO Look up augmentation

				if (r.Person.GeneralEvents.Best.NormalizedPlace != null)
				{
					PlaceNode node = PlaceAuthorityMemoryIndex.
						Instance.FindPlaceByPlaceId(r.Person.GeneralEvents.Best.NormalizedPlace.City.Id);

					//GeoLoc = new Location
					//	{
					//		X = node.Places[0].Latitude * 100,
					//		Y = node.Places[0].Longitude * 100
					//	};

					Address = node.Places.First().GetFullHierarchy();
				}				
			}

			// Y -122, 174
			//X; 34, 42

			//double hit = CensusHelper.RandomHit();

			//GeoLoc = new Location { X = 34 + (hit * 5), Y = -122 + (hit * 50) };
			
			foreach (QueryResult result in results)
			{
				if (Father != null && Father.FullName == result.Person.Name)
				{
					SelectedPersonId = result.Person.Id.Pid.ToString();
					Father.Selected = true;
					return;
				}

				if (Mother != null && Mother.FullName == result.Person.Name)
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