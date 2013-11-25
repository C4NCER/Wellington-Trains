using System.Collections.Generic;
using System.Xml.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WellingtonTrains.Web
{
	using System;
	using System.Web;
	using System.Web.UI;
	using WellingtonTrains.WebScraper;

	public partial class Default : System.Web.UI.Page
	{
		public void button1Clicked(object sender, EventArgs args)
		{
			button1.Text = "You clicked me";
		}

		public void buttonLookupClicked(object sender, EventArgs args)
		{
			TextBox1.Text = DropDownListLines.SelectedItem.Value;
		}

		DataProvider dataProvider;
		private bool swapping = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			dataProvider = new DataProvider(XDocument.Load("../WebScraper/Lines.xml"), XDocument.Load("../WebScraper/Stations.xml"), XDocument.Load("../WebScraper/StationLines.xml"));

			SetUpLine();
			SetUpStationFrom();
			SetUpStationTo();
		}

		private void SetUpLine()
		{		
			List<LineClass> lines = (List<LineClass>)dataProvider.GetLines();

			this.DropDownListLines.Items.Clear();

			foreach (LineClass line in lines)
				this.DropDownListLines.Items.Add(new ListItem(line.Name, line.Id));

			this.DropDownListLines.DataBind();       
//
//			if(loading)
//				spinnerLine.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerLine_ItemSelected);
		}

		private void SetUpStationFrom()
		{		
			List<StationClass> stations = (List<StationClass>)dataProvider.GetStationsForLine(DropDownListLines.SelectedItem.Text);

			this.DropDownListFrom.Items.Clear();

			foreach (StationClass station in stations)
				this.DropDownListFrom.Items.Add(new ListItem(station.Name, station.Id));

			this.DropDownListFrom.DataBind();

//
//			if(loading)
//				spinnerStationFrom.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerStationFrom_ItemSelected);
		}

		private void SetUpStationTo()
		{
			List<StationClass> stations = (List<StationClass>)dataProvider.GetOtherStationsForLine(DropDownListLines.SelectedItem.Text, DropDownListFrom.SelectedItem.Text);

			this.DropDownListTo.Items.Clear();

			foreach (StationClass station in stations)
				this.DropDownListTo.Items.Add(new ListItem(station.Name, station.Id));

			this.DropDownListTo.DataBind();

		}
		//		void spinnerLine_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		//		{
		//			if(!swapping && !loading)
		//				SetUpStationFrom();
		//		}
		//
		//		void spinnerStationFrom_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		//		{
		//			if(swapping)
		//				swapping = false;
		//			else if(!loading)
		//				SetUpStationTo();
		//			loading = false;
		//		}
		void SaveSettings(object sender, EventArgs e)
		{
//			var prefences = this.GetSharedPreferences("settings", FileCreationMode.Private);
//			var editor = prefences.Edit();
//			editor.Remove("line");
//			editor.Remove("stationFrom");
//			editor.Remove("stationTo");
//			editor.PutInt("line", spinnerLine.SelectedItemPosition);
//			editor.PutInt("stationFrom", spinnerStationFrom.SelectedItemPosition);
//			editor.PutInt("stationTo", spinnerStationTo.SelectedItemPosition);
//			editor.Commit();
		}

		void swapDirection(object sender, EventArgs e)
		{
			swapping = true;
//			int to = spinnerStationFrom.SelectedItemPosition;
//			int from = spinnerStationTo.SelectedItemPosition;
//
//			ISpinnerAdapter temp = spinnerStationFrom.Adapter;
//			spinnerStationFrom.Adapter = spinnerStationTo.Adapter;
//			spinnerStationTo.Adapter = temp;
//
//			spinnerStationFrom.SetSelection(from);
//			spinnerStationTo.SetSelection(to);
		}

		void lookup(object sender, EventArgs e)
		{
//			if(CheckNetwork()) {
//				Trip.Line = dataProvider.GetLineClassFromName(spinnerLine.SelectedItem.ToString());
//				Trip.From = dataProvider.GetStationClassFromName(spinnerStationFrom.SelectedItem.ToString());
//				Trip.To = dataProvider.GetStationClassFromName(spinnerStationTo.SelectedItem.ToString());
//
//				DataGetter dataGetter = new DataGetter();
//				RunOnUiThread(delegate { 
//					StartActivity(typeof(TimetableList));
//				});
//			}
		}
	}
}

