using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CensusService.Models
{
	public class SimplePerson
	{
		public SimplePerson(PersonModel.Person person, int censusYear)
		{
			if (person != null)
			{
				Id = person.Id.Pid;
				FullName = person.Name;
				Gender = person.Gender.ToString();

				if (person.Births.Best != null)
				{
					BirthYear = person.Births.Best.NormalizedDate.Year;
				}

				if (person.Deaths.Best != null)
				{
					DeathYear = person.Deaths.Best.NormalizedDate.Year;
				}

				CensusYear = censusYear;

				Age = CensusYear - BirthYear;
			}
		}

		public long Id { get; set; }
		public string Gender { get; set; }
		public string FullName { get; set; }
		public int CensusYear { get; set; }
		public int BirthYear { get; set; }
		public int DeathYear { get; set; }
		public int Age { get; set; }
	}
}