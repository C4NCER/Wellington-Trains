using System;

namespace WellingtonTrains.WebScraper
{
	public class Trip
	{
		private static Trip instance;
		public static LineClass Line { get; set; }
		public static StationClass From { get; set; }
		public static StationClass To { get; set; }
		public static String Direction { get { return From.Rank > To.Rank ? "Inbound" : "Outbound"; } }

		private Trip() { }

		public static Trip Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Trip();
				}
				return instance;
			}
		}

	}
}
