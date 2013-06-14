using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WellingtonTrains
{
	[Activity (Label = "@string/app_name")]			
		public class TimetableDayActivity : Activity
	{

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			ScrollView scrollView = new ScrollView(this);

			TableLayout tableLayout = new TableLayout(this);
			TableRow tablerow = new TableRow(this);

			// make columns span the whole width
			tableLayout.SetColumnStretchable(0, true);
			tableLayout.SetColumnStretchable(1, true);
			
			TextView DepartCollumn = new TextView(this);
			DepartCollumn.Text = "Depart";
			tablerow.AddView(DepartCollumn);
			TimetableList.TimeColumns.Add(DepartCollumn);
			TextView ArriveCollumn = new TextView(this);
			ArriveCollumn.Text = "Arrive";
			tablerow.AddView(ArriveCollumn);
			TimetableList.TimeColumns.Add(ArriveCollumn);

			tableLayout.AddView(tablerow);
//			tableLayout.SetScrollContainer(true);

			scrollView.AddView(tableLayout);
			
			SetContentView(scrollView);
		}
	}
}

