using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Phone.Net.NetworkInformation;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace WellingtonTrains
{
    public partial class LocalDB_WP7 : PhoneApplicationPage
    {
        const string ConnectionString = "Data Source=isostore:/TrainsDB.sdf";
        DataProvider DataProvider;
        private bool loading = true;
        public LocalDB_WP7()
        {
            InitializeComponent();

            DataProvider = new DataProvider();

            InputScope _scope = new InputScope();
            InputScopeName _scopeName = new InputScopeName();
            _scopeName.NameValue = InputScopeNameValue.Default;
            _scope.Names.Add(_scopeName);

            LoadData();


            this.lineListPicker.SelectionChanged += new SelectionChangedEventHandler(addEventListeners);
            this.stationFromListPicker.SelectionChanged += new SelectionChangedEventHandler(addEventListeners);
            this.stationToListPicker.SelectionChanged += new SelectionChangedEventHandler(addEventListeners);

        }

        private void addEventListeners(object sender, SelectionChangedEventArgs e)
        {
            if (!loading)
            {
                this.lineListPicker.SelectionChanged -= addEventListeners;
                this.stationFromListPicker.SelectionChanged -= addEventListeners;
                this.stationToListPicker.SelectionChanged -= addEventListeners;

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
            stationFromListPicker.ItemsSource = DataProvider.GetStations((LineClass)lineListPicker.SelectedItem);
            stationToListPicker.ItemsSource = DataProvider.GetStations((LineClass)lineListPicker.SelectedItem, (StationClass)stationFromListPicker.SelectedItem);
        }

        void lineListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!loading)
                lineSelectionChanged();
        }

        private void stationFromSelectionChanged()
        {
            stationToListPicker.ItemsSource = DataProvider.GetStations((LineClass)lineListPicker.SelectedItem, (StationClass)stationFromListPicker.SelectedItem);
        }

        void stationFromListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!loading)
                stationFromSelectionChanged();
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
                stationToSelectionChanged();
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

        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                try
                {
                    string the_page = reader.ReadToEnd();
                    the_page = the_page.Substring(the_page.IndexOf("<div class=\"timetableContainer\">"));
                    the_page = the_page.Substring(the_page.IndexOf("<ul>") + 4);
                    the_page = the_page.Substring(the_page.IndexOf("<ul>") + 4);
                    the_page = the_page.Substring(0, the_page.IndexOf("</ul>"));
                    the_page = the_page.Replace("\r", "").Replace("\n", "").Replace("/tr><tr", "/tr>derp<tr");

                    string[] parts = Regex.Split(the_page, "derp|<tr class=\'trKey'>.(.)*?</td></tr>|Peak|</?td.*?>|</?table.*?>|</?li.*?>|<sup>(T|X)</sup>|class='[a-zA-z]*'");

                    List<string> partsList = new List<string>();

                    for (int i = 0; i < parts.Length; i++)
                        if (parts[i].Length > 2)
                            partsList.Add(parts[i]);

                    List<String> depart = new List<string>();
                    List<String> arrive = new List<string>();

                    for (int i = 0; i < partsList.Count; i++)
                    {
                        if (partsList[i].Substring(0, 3) == "<tr")
                            while (partsList[++i].Substring(0, 4) != "</tr")
                                depart.Add(partsList[i]);
                        i++;
                        if (partsList[i].Substring(0, 3) == "<tr")
                            while (partsList[++i].Substring(0, 4) != "</tr")
                                arrive.Add(partsList[i]);
                    }


                    String departTimes = "";
                    String arriveTimes = "";

                    for (int i = 0; i < depart.Count; i++)
                    {
                        departTimes += depart[i].Trim() + "\r";
                        arriveTimes += arrive[i].Trim() + "\r";
                    }

                    NavigationService.Navigate(new Uri("/TimetableList.xaml?departTimes=" + departTimes + "&arriveTimes=" + arriveTimes + "&line=" + ((LineClass)lineListPicker.SelectedItem).Id + "&stationFrom=" + ((StationClass)stationFromListPicker.SelectedItem).Id + "&stationTo=" + ((StationClass)stationToListPicker.SelectedItem).Id, UriKind.RelativeOrAbsolute));
                }
                catch (Exception err)
                {
                    ToastPrompt toast = new ToastPrompt();
                    toast.Title = "Uh oh";
                    toast.Message = err.Message;
                    toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
                    toast.MillisecondsUntilHidden = 3000;
                    toast.Show();
                }
            }

        }

        private void lookup(object sender, EventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                var webClient = new WebClient();

                string url = "http://www.tranzmetro.co.nz/Timetable.aspx?LineID=" + ((LineClass)lineListPicker.SelectedItem).Id + "&DirectionID=" + (((StationClass)stationFromListPicker.SelectedItem).Id < ((StationClass)stationToListPicker.SelectedItem).Id ? 0 : 1) + "&DayID=0&FromID=" + ((StationClass)stationFromListPicker.SelectedItem).Id + "&ToID=" + ((StationClass)stationToListPicker.SelectedItem).Id;

                webClient.Headers["User-Agent"] = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)";
                webClient.OpenReadAsync(new Uri(url));
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
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