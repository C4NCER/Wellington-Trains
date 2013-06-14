using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace WellingtonTrains.WebScraper
{
	public partial class DataProvider
	{
		XDocument LineData;
		XDocument StationData;
		XDocument StationLineData;

		public DataProvider(XDocument LineData, XDocument StationData, XDocument StationlineData)
		{
			this.LineData = LineData;
			this.StationData = StationData;
			this.StationLineData = StationlineData;
		}

		public IList<LineClass> GetLines()
		{
			var data = from query in LineData.Descendants("Line")
								 select new LineClass
								 {
									 Id = (string)query.Element("Id"),
									 Name = (string)query.Element("Name")
								 };
			return data.ToList<LineClass>();
		}

		public IList<StationClass> GetAllStations()
		{
			var data = from query in StationData.Descendants("Station")
								 select new StationClass
								 {
									 Id = (string)query.Element("Id"),
									 Name = (string)query.Element("Name"),
									 Rank = (int)query.Element("Rank")
								 };
			return data.ToList<StationClass>();
		}

		public IList<StationClass> GetStationsForLine(string lineName)
		{
			foreach (LineClass l in GetLines())
				if (l.Name == lineName)
					return GetStationsForLine(l);
			return null;
		}
		
		public IList<StationClass> GetStationsForLine(LineClass line)
		{
			var data = from station in StationData.Descendants("Station")
				join stationLine in StationLineData.Descendants("StationLine") on (string)station.Element("Id") equals (string)stationLine.Element("StationId")
					where (string)stationLine.Element("LineId") == line.Id
					select new StationClass
			{
				Id = (string)station.Element("Id"),
				Name = (string)station.Element("Name"),
				Rank = (int)station.Element("Rank")
			};
			return data.ToList<StationClass>();
		}
		
		public IList<StationClass> GetOtherStationsForLine(string lineName, string stationName)
		{
			foreach (LineClass l in GetLines())
				if (l.Name == lineName)
					foreach (StationClass s in GetStationsForLine(l))
						if (s.Name == stationName)
							return GetOtherStationsForLine(l, s);
			return null;
		}

		public IList<StationClass> GetOtherStationsForLine(LineClass line, StationClass station)
		{
			var data = from s in StationData.Descendants("Station")
				join stationLine in StationLineData.Descendants("StationLine") on (string)s.Element("Id") equals (string)stationLine.Element("StationId")
					where (string)stationLine.Element("LineId") == line.Id && (string)s.Element("Id") != station.Id
					select new StationClass
			{
				Id = (string)s.Element("Id"),
				Name = (string)s.Element("Name"),
				Rank = (int)s.Element("Rank")
			};
			return data.ToList<StationClass>();
		}

		public IList<StationLineClass> GetStationLines()
		{
			var data = from query in StationLineData.Descendants("StationLine")
								 select new StationLineClass
								 {
									 StationId = (string)query.Element("StationId"),
									 LineId = (string)query.Element("LineId")
								 };
			return data.ToList<StationLineClass>();
		}

		public StationClass GetStationClassFromName(string stationName)
		{
			foreach (StationClass station in GetAllStations())
				if (station.Name == stationName)
					return station;
			return null;
		}
		
		public LineClass GetLineClassFromName(string lineName)
		{
			foreach (LineClass line in GetLines())
				if (line.Name == lineName)
					return line;
			return null;
		}

		class StationComparer : IEqualityComparer<StationClass>
		{
			public bool Equals(StationClass x, StationClass y)
			{
				return x.Id.Equals(y.Id);
			}

			public int GetHashCode(StationClass obj)
			{
				return obj.Id.GetHashCode();
			}
		}

	}

}