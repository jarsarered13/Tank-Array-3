using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonModel;

namespace CensusService.Models
{
	public class SimpleFamily
	{
		private List<SimplePerson> _children = null;

		public int Version { get; set; }
		public long id { get; set; }
		public int CensusYear { get; set; }
		public string SearchPersonId { get; set; }
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
	}
}