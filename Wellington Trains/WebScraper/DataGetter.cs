using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace WellingtonTrains.WebScraper
{
	public enum Day
	{
		Today
			,
		Monday
			,
		Tuesday
			,
		Wednesday
			,
		Thursday
			,
		Friday
			,
		Saturday
			,
		Sunday
	}
	;

	public enum Month
	{
		January = 1
			,
		February
			,
		March
			,
		April
			,
		May
			,
		June
			,
		July
			,
		August
			,
		September
			,
		October
			,
		November
			,
		December
	}
	;

	public class DataGetter
	{
		private DateTime the_past;
		public static StringEventFirer StringEventHandler;

		public DataGetter()
		{
			StringEventHandler = new StringEventFirer();

			the_past = stringToDate("1984/09/02 12:34 pm");
			GoGoGadget();
		}

		private void GoGoGadget()
		{
			for (int daysToAdd = 0; daysToAdd < 8; daysToAdd++)
			{
				var webClient = new WebClient();

				string url = "http://www.metlink.org.nz/timetables/train/" + Trip.Line.Id + "/" + Trip.Direction + "/" + "?date=" + DateTime.Today.AddDays((double)daysToAdd).ToString("M/d/yyyy") + "&allStops=1";

				webClient.Headers["User-Agent"] = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
				webClient.DownloadStringAsync(new Uri(url));
				webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadDataCompleted);
			}
		}

		void webClient_DownloadDataCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				int day = 8;
				List<DateTime> depart = new List<DateTime>();
				List<DateTime> arrive = new List<DateTime>();
				try
				{
					string the_page = e.Result;
					if (the_page.IndexOf("This timetable is valid for ") > 0)
					{
						the_page = the_page.Substring(the_page.IndexOf("This timetable is valid for "));
						the_page = the_page.Substring(the_page.IndexOf(">") + 1);
					} else if (the_page.IndexOf("This service does not have a timetable for ") > 0)
					{
						the_page = the_page.Substring(the_page.IndexOf("This service does not have a timetable for ") + "This service does not have a timetable for ".Length);
					}
					the_page = the_page.Substring(the_page.IndexOf(" ") + 1);
					string day_string = the_page.Substring(0, the_page.IndexOf("<")).Trim().Trim('.');
					String month_string = day_string.Substring(day_string.IndexOf(" ")).Trim();
					int month = -1;
					switch (month_string)
					{
						case "Jan":
							month = 1;
							break;
						case "Feb":
							month = 2;
							break;
						case "Mar":
							month = 3;
							break;
						case "Apr":
							month = 4;
							break;
						case "May":
							month = 5;
							break;
						case "Jun":
							month = 6;
							break;
						case "Jul":
							month = 7;
							break;
						case "Aug":
							month = 8;
							break;
						case "Sep":
							month = 9;
							break;
						case "Oct":
							month = 10;
							break;
						case "Nov":
							month = 11;
							break;
						case "Dec":
							month = 12;
							break;
					}
					DateTime daydt = new DateTime(DateTime.Now.Year, month, int.Parse(day_string.Substring(0, day_string.IndexOf(" "))));
					day = (daydt.DayOfYear - DateTime.Now.DayOfYear) % 365;
					if (day != 0)
					{
						day = (int)daydt.DayOfWeek;
						if (day == 0)
						{
							day = 7;
						}
					}
					the_page = the_page.Substring(the_page.IndexOf(@"<div id=""" + Trip.Direction));
					List<String> bfgds = getTimeTableRows(the_page);
					foreach (String timeTableRow in bfgds)
					{
						if (timeTableRow.IndexOf("name=\"" + Trip.From.Id + "\"") != -1)
							depart = getTrainTimes(timeTableRow, depart, day);
						else if (timeTableRow.IndexOf("name=\"" + Trip.To.Id + "\"") != -1)
							arrive = getTrainTimes(timeTableRow, arrive, day);
					}
				} catch (Exception err)
				{
					if ((day == 6 || day == 7) && Trip.Line.Id == "MEL")
					{
						StringEventHandler.SetDepartTimes(day, "No trains." + err);
					}
				}

				ParseData(depart, arrive, day);
			} catch (WebException err)
			{
				WebResponse herp = err.Response;
			}
		}

		private WellingtonTrains.WebScraper.Day getDay(DateTime dateTime)
		{
			if (dateTime == DateTime.Today)
				return 0;
			else
				return (WellingtonTrains.WebScraper.Day)((int)dateTime.DayOfWeek == 0 ? 7 : (int)dateTime.DayOfWeek);
		}

		private List<String> getTimeTableRows(string HTML)
		{
			return new List<string>(Regex.Split(HTML, "\\n<tr.*?timing\\\">"));
		}

		private List<DateTime> getTrainTimes(string HTML, List<DateTime> list, int day)
		{
			MatchCollection herp = Regex.Matches(HTML, "\\<td.*\\/td>");
			foreach (Match derp in herp)
			{
				MatchCollection spans = Regex.Matches(derp.Value, "\\d{1,2}:\\d{2} (a|p)m");
				int count = list.Count;
				foreach (Match span in spans)
				{
					DateTime time = stringToDate(DateTime.Now.AddDays(day).ToString("yyyy/MM/dd") + " " + span.Value);
					if (list.IndexOf(time) == -1)
						list.Add(time);
				}
				if (list.Count == count)
				{
					list.Add(the_past);
				}
			}
			return list;
		}

		private DateTime stringToDate(string dateAsString)
		{
			char[] delimiters = new char[] { '/', ':', ' ' };
			string[] parts = dateAsString.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			DateTime date = new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), parts[5] == "am" ? int.Parse(parts[3]) % 12 : parts[3] == "12" ? 12 : int.Parse(parts[3]) + 12, int.Parse(parts[4]), 0);
			return date;
		}

		private void ParseData(List<DateTime> departs, List<DateTime> arrives, int day)
		{
			String departText = "";
			String arriveText = "";
			for (int i = 0; i < departs.Count; i++)
			{
				if (arrives[i] != the_past && departs[i] != the_past)
				{
					if ((day == 0 && departs[i].CompareTo(DateTime.Now) > 0) || (day >= 1 && day <= 7))
					{
						departText += departs[i].ToString("h:mm tt") + "\r";
						arriveText += arrives[i].ToString("h:mm tt") + "\r";
					}
				}
			}

			if (departText == "")
			{
				departText = "No trains.";
			}

			StringEventHandler.SetDepartTimes(day, departText);
			StringEventHandler.SetArriveTimes(day, arriveText);
		}
	}
}
