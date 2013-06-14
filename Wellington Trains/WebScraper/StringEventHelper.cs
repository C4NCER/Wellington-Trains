using System;

namespace WellingtonTrains.WebScraper
{
	public class ColumnTextData : EventArgs
	{
		public String[] TimeColumns;
	}

	public class StringEventFirer
	{
		public delegate void ChangedEventHandler(StringEventFirer sender,ColumnTextData e);

		public event ChangedEventHandler Changed;

		private ColumnTextData data;
		private String[] TimeColumns;

		public void SetDepartTimes(int day, String times)
		{
			if (day < 8)
			{
				TimeColumns[(2 * day)] = times;
				data.TimeColumns = TimeColumns;
				CheckAndSend();
			}
		}

		public void SetArriveTimes(int day, String times)
		{
			if (day < 8)
			{
				TimeColumns[(2 * day) + 1] = times;
				data.TimeColumns = TimeColumns;
				CheckAndSend();
			}
		}

		private void CheckAndSend()
		{
			foreach (string s in data.TimeColumns)
				if (s == null)
					return;
			
			Changed(this, data);
		}

		public StringEventFirer()
		{
			data = new ColumnTextData();
			TimeColumns = new String[16];
		}
	}
}
