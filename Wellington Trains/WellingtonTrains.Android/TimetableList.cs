using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WellingtonTrains.WebScraper;

namespace WellingtonTrains
{
	[Activity (Label = "@string/app_name")]			
	public class TimetableList : TabActivity
	{

		private StringEventFirer TodayDepartStringEventFirer;
		public static List<TextView> TimeColumns;
		ProgressBar loadingBar;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.TimeTable);

			TimeColumns = new List<TextView>(16);

			foreach (string day in new string[] {"Today","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"})
				TabHost.AddTab(NewTab(day));

			//magical tab jumping to actually create the content in the tabs #lolandroid
			TabHost.CurrentTab = 0;		
			TabHost.CurrentTab = 1;		
			TabHost.CurrentTab = 2;		
			TabHost.CurrentTab = 3;		
			TabHost.CurrentTab = 4;		
			TabHost.CurrentTab = 5;		
			TabHost.CurrentTab = 6;		
			TabHost.CurrentTab = 7;		
			TabHost.CurrentTab = 8;		
			TabHost.CurrentTab = 0;		

			loadingBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
			loadingBar.Indeterminate = true;

			TodayDepartStringEventFirer = DataGetter.StringEventHandler;
			if (DataGetter.StringEventHandler != null)
				DataGetter.StringEventHandler.Changed += new StringEventFirer.ChangedEventHandler(TimetableStringChanged);

		}

		private TabHost.TabSpec NewTab(String day)
		{
			TabHost.TabSpec spec;   
			Intent intent;            

			// Create an Intent to launch an Activity for the tab (to be reused)
			intent = new Intent(this, typeof(TimetableDayActivity));
			intent.AddFlags(ActivityFlags.NewTask);

			// Initialize a TabSpec for each tab and add it to the TabHost
			spec = TabHost.NewTabSpec(day);
			spec.SetIndicator(" " + day + " ");
			spec.SetContent(intent);
			return spec;
		}

		private void TimetableStringChanged(object sender, ColumnTextData e)
		{
			//hardcoded because lolandroid
			this.RunOnUiThread(() => TimeColumns[0].Text +=  "\n" + e.TimeColumns[0].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[1].Text +=  "\n" + e.TimeColumns[1].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[2].Text +=  "\n" + e.TimeColumns[2].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[3].Text +=  "\n" + e.TimeColumns[3].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[4].Text +=  "\n" + e.TimeColumns[4].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[5].Text +=  "\n" + e.TimeColumns[5].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[6].Text +=  "\n" + e.TimeColumns[6].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[7].Text +=  "\n" + e.TimeColumns[7].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[8].Text +=  "\n" + e.TimeColumns[8].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[9].Text +=  "\n" + e.TimeColumns[9].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[10].Text +=  "\n" + e.TimeColumns[10].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[11].Text +=  "\n" + e.TimeColumns[11].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[12].Text +=  "\n" + e.TimeColumns[12].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[13].Text +=  "\n" + e.TimeColumns[13].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[14].Text +=  "\n" + e.TimeColumns[14].Replace("\r", "\n"));
			this.RunOnUiThread(() => TimeColumns[15].Text +=  "\n" + e.TimeColumns[15].Replace("\r", "\n"));
			this.RunOnUiThread(() => loadingBar.Visibility = ViewStates.Gone);
		}
	}
}
