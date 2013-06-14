using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using WellingtonTrains.WebScraper;

namespace WellingtonTrains
{
	public partial class TimetableList : PhoneApplicationPage
	{

		private StringEventFirer TodayDepartStringEventFirer;

		private List<TextBlock> TimeColumns;

		public TimetableList()
		{
			InitializeComponent();

			TimeColumns = new List<TextBlock>(16);
			TimeColumns.Add(TodayDepartColumn);
			TimeColumns.Add(TodayArriveColumn);
			TimeColumns.Add(MondayDepartColumn);
			TimeColumns.Add(MondayArriveColumn);
			TimeColumns.Add(TuesdayDepartColumn);
			TimeColumns.Add(TuesdayArriveColumn);
			TimeColumns.Add(WednesdayDepartColumn);
			TimeColumns.Add(WednesdayArriveColumn);
			TimeColumns.Add(ThursdayDepartColumn);
			TimeColumns.Add(ThursdayArriveColumn);
			TimeColumns.Add(FridayDepartColumn);
			TimeColumns.Add(FridayArriveColumn);
			TimeColumns.Add(SaturdayDepartColumn);
			TimeColumns.Add(SaturdayArriveColumn);
			TimeColumns.Add(SundayDepartColumn);
			TimeColumns.Add(SundayArriveColumn);
			TodayDepartStringEventFirer = DataGetter.StringEventHandler;
			DataGetter.StringEventHandler.Changed += new StringEventFirer.ChangedEventHandler(TodayDepartStringChanged);
		}

		private void TodayDepartStringChanged(object sender, ColumnTextData e)
		{
			for (int i = 0; i < e.TimeColumns.Length; i++)
			{
				if (e.TimeColumns[i] != null)
				{
					if (e.TimeColumns[i].IndexOf("No trains.") != -1)
					{
						ShowError(i / 2, "No trains.");
					}
					if (TimeColumns[i].Text == "")
					{
						TimeColumns[i].Text = e.TimeColumns[i];
					}
				}
			}

			performanceProgressBar.Visibility = Visibility.Collapsed;
		}

		private void ShowError(int day, string message)
		{
			switch (day)
			{
				case 0:
					TodayTimes.Visibility = Visibility.Collapsed;
					TodayError.Visibility = Visibility.Visible;
					break;
				case 1:
					MondayTimes.Visibility = Visibility.Collapsed;
					MondayError.Visibility = Visibility.Visible;
					break;
				case 2:
					TuesdayTimes.Visibility = Visibility.Collapsed;
					TuesdayError.Visibility = Visibility.Visible;
					break;
				case 3:
					WednesdayTimes.Visibility = Visibility.Collapsed;
					WednesdayError.Visibility = Visibility.Visible;
					break;
				case 4:
					ThursdayTimes.Visibility = Visibility.Collapsed;
					ThursdayError.Visibility = Visibility.Visible;
					break;
				case 5:
					FridayTimes.Visibility = Visibility.Collapsed;
					FridayError.Visibility = Visibility.Visible;
					break;
				case 6:
					SaturdayTimes.Visibility = Visibility.Collapsed;
					SaturdayError.Visibility = Visibility.Visible;
					break;
				case 7:
					SundayTimes.Visibility = Visibility.Collapsed;
					SundayError.Visibility = Visibility.Visible;
					break;
			}
		}

		protected override void OnNavigatedTo
		(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
		}
	}
}