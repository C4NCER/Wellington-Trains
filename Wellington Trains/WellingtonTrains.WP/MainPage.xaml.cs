using System;
using System.Collections;
using System.IO.IsolatedStorage;
using System.Windows.Controls;
using System.Windows.Input;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using WellingtonTrains.WebScraper;
using System.Xml.Linq;

namespace WellingtonTrains
{
	public partial class TrainApp : PhoneApplicationPage
	{
		DataProvider DataProvider;
		private bool loading = true;

		public TrainApp()
		{
			InitializeComponent();

			DataProvider = new DataProvider(XDocument.Load("Lines.xml"),XDocument.Load("Stations.xml"),XDocument.Load("StationLines.xml"));

			LoadData();

			this.lineListPicker.SelectionChanged += new SelectionChangedEventHandler(AddEventListeners);
			this.stationFromListPicker.SelectionChanged += new SelectionChangedEventHandler(AddEventListeners);
			this.stationToListPicker.SelectionChanged += new SelectionChangedEventHandler(AddEventListeners);
		}

		private void AddEventListeners(object sender, SelectionChangedEventArgs e)
		{
			if (!loading)
			{
				this.lineListPicker.SelectionChanged -= AddEventListeners;
				this.stationFromListPicker.SelectionChanged -= AddEventListeners;
				this.stationToListPicker.SelectionChanged -= AddEventListeners;

				this.lineListPicker.SelectionChanged -= lineListPicker_SelectionChanged;
				this.stationFromListPicker.SelectionChanged -= stationFromListPicker_SelectionChanged;
				this.stationToListPicker.SelectionChanged -= stationToListPicker_SelectionChanged;

				this.lineListPicker.SelectionChanged += new SelectionChangedEventHandler(lineListPicker_SelectionChanged);
				this.stationFromListPicker.SelectionChanged += new SelectionChangedEventHandler(stationFromListPicker_SelectionChanged);
				this.stationToListPicker.SelectionChanged += new SelectionChangedEventHandler(stationToListPicker_SelectionChanged);
			}
			loading = false;
		}

		private void lineSelectionChanged()
		{
			stationFromListPicker.ItemsSource = DataProvider.GetStationsForLine((LineClass)lineListPicker.SelectedItem);
			stationToListPicker.ItemsSource = DataProvider.GetOtherStationsForLine((LineClass)lineListPicker.SelectedItem, (StationClass)stationFromListPicker.SelectedItem);
		}

		void lineListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!loading)
			{
				lineSelectionChanged();
			}
		}

		private void stationFromSelectionChanged()
		{
			stationToListPicker.ItemsSource = DataProvider.GetOtherStationsForLine((LineClass)lineListPicker.SelectedItem, (StationClass)stationFromListPicker.SelectedItem);
		}

		void stationFromListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!loading)
			{
				stationFromSelectionChanged();
			}
		}

		private void stationToSelectionChanged()
		{
			if (lineListPicker.SelectedItem.ToString() == "")
			{
				return;
			}
		}

		void stationToListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!loading)
			{
				stationToSelectionChanged();
			}
		}

		public void LoadData()
		{
			this.lineListPicker.ItemsSource = DataProvider.GetLines();

			if (IsolatedStorageSettings.ApplicationSettings.Contains("Line"))
			{
				string herp = (String)IsolatedStorageSettings.ApplicationSettings["Line"];
				for (int i = 0; i < lineListPicker.Items.Count; i++)
				{
					if (((LineClass)lineListPicker.Items[i]).Name == herp)
					{
						lineListPicker.SelectedIndex = i;
						break;
					}
				}
			}
			else
			{
				lineListPicker.SelectedIndex = 0;
			}
			lineSelectionChanged();

			if (IsolatedStorageSettings.ApplicationSettings.Contains("StationFrom"))
			{
				string herp = (String)IsolatedStorageSettings.ApplicationSettings["StationFrom"];
				for (int i = 0; i < stationFromListPicker.Items.Count; i++)
				{
					if (((StationClass)stationFromListPicker.Items[i]).Name == herp)
					{
						stationFromListPicker.SelectedIndex = i;
						break;
					}
				}
			}
			else
			{
				stationFromListPicker.SelectedIndex = 0;
			}
			stationFromSelectionChanged();

			if (IsolatedStorageSettings.ApplicationSettings.Contains("StationTo"))
			{
				string herp = (String)IsolatedStorageSettings.ApplicationSettings["StationTo"];
				for (int i = 0; i < stationToListPicker.Items.Count; i++)
				{
					if (((StationClass)stationToListPicker.Items[i]).Name == herp)
					{
						stationToListPicker.SelectedIndex = i;
						break;
					}
				}
			}
			else
			{
				stationToListPicker.SelectedIndex = 0;
			}
			stationToSelectionChanged();
		}

		private void lookup(object sender, EventArgs e)
		{
			if (NetworkInterface.GetIsNetworkAvailable())
			{
				Trip.Line = (LineClass)lineListPicker.SelectedItem;
				Trip.From = (StationClass)stationFromListPicker.SelectedItem;
				Trip.To = (StationClass)stationToListPicker.SelectedItem;
				String direction = Trip.From.Rank > Trip.To.Rank ? "Inbound" : "Outbound";

				DataGetter dataGetter = new DataGetter();
				NavigationService.Navigate(new Uri("/TimetableList.xaml", UriKind.RelativeOrAbsolute));
			}
			else
			{
				ToastPrompt toast = new ToastPrompt();
				toast.Title = "No Network Detected";
				toast.Message = "A network connection is required to \rdownload the train timetable.";
				toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
				toast.MillisecondsUntilHidden = 3000;
				toast.Show();
			}
		}

		private void SaveDefaults(object sender, EventArgs e)
		{
			if (!IsolatedStorageSettings.ApplicationSettings.Contains("Line"))
				IsolatedStorageSettings.ApplicationSettings.Add("Line", ((LineClass)lineListPicker.SelectedItem).Name);
			else
				IsolatedStorageSettings.ApplicationSettings["Line"] = ((LineClass)lineListPicker.SelectedItem).Name;

			if (!IsolatedStorageSettings.ApplicationSettings.Contains("StationFrom"))
				IsolatedStorageSettings.ApplicationSettings.Add("StationFrom", ((StationClass)stationFromListPicker.SelectedItem).Name);
			else
				IsolatedStorageSettings.ApplicationSettings["StationFrom"] = ((StationClass)stationFromListPicker.SelectedItem).Name;

			if (!IsolatedStorageSettings.ApplicationSettings.Contains("StationTo"))
				IsolatedStorageSettings.ApplicationSettings.Add("StationTo", ((StationClass)stationToListPicker.SelectedItem).Name);
			else
				IsolatedStorageSettings.ApplicationSettings["StationTo"] = ((StationClass)stationToListPicker.SelectedItem).Name;

			IsolatedStorageSettings.ApplicationSettings.Save();
		}

		private void SwapDirections(object sender, EventArgs e)
		{
			loading = true;

			IEnumerable temp = stationFromListPicker.ItemsSource;
			int fromIndex = stationFromListPicker.SelectedIndex;

			stationFromListPicker.ItemsSource = stationToListPicker.ItemsSource;
			stationFromListPicker.SelectedIndex = stationToListPicker.SelectedIndex;

			stationToListPicker.ItemsSource = temp;
			stationToListPicker.SelectedIndex = fromIndex;

			loading = false;
		}
	}
}