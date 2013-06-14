using System;
using System.Net;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using WellingtonTrains.WebScraper;
using Android.Net;

namespace WellingtonTrains
{
	[Activity (Label = "@string/app_name", MainLauncher = true)]
	public class Activity1 : Activity
	{
		DataProvider dataProvider;
		Spinner spinnerLine;
		Spinner spinnerStationFrom;
		Spinner spinnerStationTo;
		private bool swapping = false;
		private bool loading = true;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			dataProvider = new DataProvider(XDocument.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("Lines.xml")), XDocument.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("Stations.xml")), XDocument.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("StationLines.xml")));

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			
			spinnerLine = FindViewById<Spinner>(Resource.Id.spinnerLine);
			spinnerStationFrom = FindViewById<Spinner>(Resource.Id.spinnerStationFrom);
			spinnerStationTo = FindViewById<Spinner>(Resource.Id.spinnerStationTo);
			
			Button buttonSwap = FindViewById<Button>(Resource.Id.buttonSwap);
			buttonSwap.Click += new EventHandler(swapDirection);

			Button buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
			buttonSave.Click += new EventHandler(SaveSettings);

			Button buttonSearch = FindViewById<Button>(Resource.Id.buttonSearch);
			buttonSearch.Click += new EventHandler(lookup);

			SetUpLine();
			SetUpStationFrom();
			SetUpStationTo();
		}

		private void SetUpLine()
		{		
			List<LineClass> lines = (List<LineClass>)dataProvider.GetLines();
			
			ArrayAdapter<LineClass> adapter = new ArrayAdapter<LineClass>(this, Android.Resource.Layout.SimpleSpinnerItem);           
			foreach (LineClass l in lines) 
				adapter.Add(l.Name);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerLine.Adapter = adapter;

			if (loading)
			{
				var prefences = this.GetSharedPreferences("settings", FileCreationMode.Private);
				int herp = prefences.GetInt("line", 0);
				spinnerLine.SetSelection(herp);
			} else
			{
				spinnerLine.SetSelection(0);
			}

			if (loading)
				spinnerLine.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerLine_ItemSelected);
		}

		private void SetUpStationFrom()
		{		
			List<StationClass> stations = (List<StationClass>)dataProvider.GetStationsForLine(spinnerLine.SelectedItem.ToString());
			
			ArrayAdapter<StationClass> adapter = new ArrayAdapter<StationClass>(this, Android.Resource.Layout.SimpleSpinnerItem);           
			foreach (StationClass s in stations) 
				adapter.Add(s.Name);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerStationFrom.Adapter = adapter;

			if (loading)
			{
				var prefences = this.GetSharedPreferences("settings", FileCreationMode.Private);
				spinnerStationFrom.SetSelection(prefences.GetInt("stationFrom", 0));
			} else
			{
				spinnerStationFrom.SetSelection(0);
			}

			if (loading)
				spinnerStationFrom.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerStationFrom_ItemSelected);
		}

		private void SetUpStationTo()
		{
			List<StationClass> stations = (List<StationClass>)dataProvider.GetOtherStationsForLine(spinnerLine.SelectedItem.ToString(), spinnerStationFrom.SelectedItem.ToString());
			
			ArrayAdapter<StationClass> adapter = new ArrayAdapter<StationClass>(this, Android.Resource.Layout.SimpleSpinnerItem);           
			foreach (StationClass s in stations) 
				adapter.Add(s.Name);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerStationTo.Adapter = adapter;

			if (loading)
			{
				var prefences = this.GetSharedPreferences("settings", FileCreationMode.Private);			
				spinnerStationTo.SetSelection(prefences.GetInt("stationTo", 0));
			} else
			{
				spinnerStationTo.SetSelection(0);
			}
		}

		private bool CheckNetwork()
		{
			ConnectivityManager conMgr = (ConnectivityManager)ApplicationContext.GetSystemService(Context.ConnectivityService);
			
			bool output = conMgr.ActiveNetworkInfo != null && conMgr.ActiveNetworkInfo.IsAvailable && conMgr.ActiveNetworkInfo.IsConnected;
			
			conMgr.Dispose();
			
			return output;
		}

		void spinnerLine_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			if (!swapping && !loading)
				SetUpStationFrom();
		}

		void spinnerStationFrom_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			if (swapping) 
				swapping = false;
			else if (!loading)
				SetUpStationTo();
			loading = false;
		}

		void SaveSettings(object sender, EventArgs e)
		{
			var prefences = this.GetSharedPreferences("settings", FileCreationMode.Private);
			var editor = prefences.Edit();
			editor.Remove("line");
			editor.Remove("stationFrom");
			editor.Remove("stationTo");
			editor.PutInt("line", spinnerLine.SelectedItemPosition);
			editor.PutInt("stationFrom", spinnerStationFrom.SelectedItemPosition);
			editor.PutInt("stationTo", spinnerStationTo.SelectedItemPosition);
			editor.Commit();
		}

		void swapDirection(object sender, EventArgs e)
		{
			swapping = true;
			int to = spinnerStationFrom.SelectedItemPosition;
			int from = spinnerStationTo.SelectedItemPosition;

			ISpinnerAdapter temp = spinnerStationFrom.Adapter;
			spinnerStationFrom.Adapter = spinnerStationTo.Adapter;
			spinnerStationTo.Adapter = temp;

			spinnerStationFrom.SetSelection(from);
			spinnerStationTo.SetSelection(to);
		}

		void lookup(object sender, EventArgs e)
		{
			if (CheckNetwork())
			{
				Trip.Line = dataProvider.GetLineClassFromName(spinnerLine.SelectedItem.ToString());
				Trip.From = dataProvider.GetStationClassFromName(spinnerStationFrom.SelectedItem.ToString());
				Trip.To = dataProvider.GetStationClassFromName(spinnerStationTo.SelectedItem.ToString());
					
				DataGetter dataGetter = new DataGetter();
				RunOnUiThread(delegate { 
					StartActivity(typeof(TimetableList));
				});
			}
		}
	}
}


