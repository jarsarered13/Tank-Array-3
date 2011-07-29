using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CensusService.Models
{
	public enum CensusNavDirection
	{
		Previous = 1,
		Present,
		Next
	}

	public class CensusHelper
	{
		private static IDictionary<int, int> _censusYearLookups = new Dictionary<int, int>();

		private static Random random = new Random();
		
		//private static IDictionary<int, int> _databaseIdLookups = new Dictionary<int, int>();
		
		static CensusHelper()
		{
			if(_censusYearLookups == null)
			{
				_censusYearLookups = new Dictionary<int, int>();
			}

			_censusYearLookups.Add(6224, 1930);
			_censusYearLookups.Add(6061, 1920);
			_censusYearLookups.Add(7884, 1910);
			_censusYearLookups.Add(7602, 1900);
			_censusYearLookups.Add(6742, 1880);
			_censusYearLookups.Add(7163, 1870);
			_censusYearLookups.Add(7667, 1860);
			_censusYearLookups.Add(8054, 1850);
			_censusYearLookups.Add(8057, 1840);
			_censusYearLookups.Add(8058, 1830);
			_censusYearLookups.Add(7734, 1820);
			_censusYearLookups.Add(7613, 1810);
			_censusYearLookups.Add(7590, 1800);
			_censusYearLookups.Add(5058, 1790);

			//_databaseIdLookups.Add(1930, 6224);
			//_databaseIdLookups.Add(1920, 6061);
			//_databaseIdLookups.Add(1910, 7884);
			//_databaseIdLookups.Add(1900, 7602);
			//_databaseIdLookups.Add(1880, 6742);
			//_databaseIdLookups.Add(1870, 7163);
			//_databaseIdLookups.Add(1860, 7667);
			//_databaseIdLookups.Add(1850, 8054);
			//_databaseIdLookups.Add(1840, 8057);
			//_databaseIdLookups.Add(1830, 8058);
			//_databaseIdLookups.Add(1820, 7734);
			//_databaseIdLookups.Add(1810, 7613);
			//_databaseIdLookups.Add(1800, 7590);
			//_databaseIdLookups.Add(1790, 5058);

		}

		public static int GetCensusYear(int databaseId)
		{
			if (_censusYearLookups.ContainsKey(databaseId))
			{
				return _censusYearLookups[databaseId];
			}

			return 0;
		}

		public static int GetDatabaseId(int currentCensusYear, string direction)
		{
			int databaseId = 0;
			KeyValuePair<int, int> result;

			switch(direction.ToLower())
			{
				case "prev":
					result =  _censusYearLookups.Where(x => x.Value < currentCensusYear).FirstOrDefault();					
					break;				
				case "next":
					result = _censusYearLookups.Where(x => x.Value > currentCensusYear).FirstOrDefault();
					break;
				case "present":
					result = _censusYearLookups.Where(x => x.Value == currentCensusYear).FirstOrDefault();
					break;
				default:
					result = _censusYearLookups.Where(x => x.Value == currentCensusYear).FirstOrDefault();
					break;
			}
			
			return result.Key;
		}

		public static double RandomHit()
		{
			return random.NextDouble();
		}

	}
}