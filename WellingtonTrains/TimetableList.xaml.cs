using System;
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
using Microsoft.Phone.Net.NetworkInformation;
using Coding4Fun.Phone.Controls;
using System.IO;
using System.Text.RegularExpressions;

namespace WellingtonTrains
{
    public partial class TimetableList : PhoneApplicationPage
    {
        protected string Line;
        protected string From;
        protected string To;

        public TimetableList()
        {
            InitializeComponent();
        }

        private void LoadNormalDays()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                for (int i = 1; i <= 7; i++)
                {
                    var webClient = new WebClient();

                    string url = "http://www.tranzmetro.co.nz/Timetable.aspx?LineID=" + Line + "&DirectionID=" + (Convert.ToInt16(From) < Convert.ToInt16(To) ? 0 : 1) + "&DayID=" + i + "&FromID=" + From + "&ToID=" + To;

                    webClient.Headers["User-Agent"] = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                    webClient.OpenReadAsync(new Uri(url));
                    webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
                }
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

        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string day = "";
                try
                {
                    string the_page = reader.ReadToEnd();
                    the_page = the_page.Substring(the_page.IndexOf("http://www.tranzmetro.co.nz/Timetable.aspx?LineID") + 58);
                    day = the_page.Substring(0, 1);
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

                    ParseData(departTimes, arriveTimes, day);
                }
                catch (Exception err)
                {
                    ShowError(Convert.ToInt16(day));
                }
            }

        }

        private void ShowError(int day)
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
                default:
                    ToastPrompt toast = new ToastPrompt();
                    toast.Title = "Uh oh";
                    toast.Message = "Unable to derp";
                    toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
                    toast.MillisecondsUntilHidden = 3000;
                    toast.Show();
                    break;
            }
        }

        void ParseData(String departTime, String arriveTime, String day)
        {
            char[] splits = new char[1] { '\r' };
            string[] depart = departTime.Split(splits, StringSplitOptions.RemoveEmptyEntries);
            string[] arrive = arriveTime.Split(splits, StringSplitOptions.RemoveEmptyEntries);

            List<DateTime> departs = new List<DateTime>();
            List<DateTime> arrives = new List<DateTime>();

            int hours;
            int minutes;

            for (int i = 0; i < depart.Length; i++)
            {
                hours = Convert.ToInt32(depart[i].Substring(0, depart[i].IndexOf(':')));
                minutes = Convert.ToInt32(depart[i].Substring(depart[i].IndexOf('m') - 3, 2));
                departs.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, depart[i].IndexOf('p') == -1 ? hours : hours == 12 ? 12 : hours + 12, minutes, 0));

                hours = Convert.ToInt32(arrive[i].Substring(0, arrive[i].IndexOf(':')));
                minutes = Convert.ToInt32(arrive[i].Substring(arrive[i].IndexOf('m') - 3, 2));
                arrives.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, arrive[i].IndexOf('p') == -1 ? hours : hours == 12 ? 12 : hours + 12, minutes, 0));
            }

            for (int i = 0; i < departs.Count; i++)
            {
                switch (Convert.ToInt16(day))
                {
                    case 0:
                        if (departs[i].CompareTo(DateTime.Now) > 0)
                        {
                            TodayDepartColumn.Text += departs[i].ToString("h:mm tt") + "\r";
                            TodayArriveColumn.Text += arrives[i].ToString("h:mm tt") + "\r";
                        }
                        break;
                    case 1:
                        MondayDepartColumn.Text += departs[i].ToString("h:mm tt") + "\r";
                        MondayArriveColumn.Text += arrives[i].ToString("h:mm tt") + "\r";
                        break;
                    case 2:
                        TuesdayDepartColumn.Text += departs[i].ToString("h:mm tt") + "\r";
                        TuesdayArriveColumn.Text += arrives[i].ToString("h:mm tt") + "\r";
                        break;
                    case 3:
                        WednesdayDepartColumn.Text += departs[i].ToString("h:mm tt") + "\r";
                        WednesdayArriveColumn.Text += arrives[i].ToString("h:mm tt") + "\r";
                        break;
                    case 4:
                        ThursdayDepartColumn.Text += departs[i].ToString("h:mm tt") + "\r";
                        ThursdayArriveColumn.Text += arrives[i].ToString("h:mm tt") + "\r";
                        break;
                    case 5:
                        FridayDepartColumn.Text += departs[i].ToString("h:mm tt") + "\r";
                        FridayArriveColumn.Text += arrives[i].ToString("h:mm tt") + "\r";
                        break;
                    case 6:
                        SaturdayDepartColumn.Text += departs[i].ToString("h:mm tt") + "\r";
                        SaturdayArriveColumn.Text += arrives[i].ToString("h:mm tt") + "\r";
                        break;
                    case 7:
                        SundayDepartColumn.Text += departs[i].ToString("h:mm tt") + "\r";
                        SundayArriveColumn.Text += arrives[i].ToString("h:mm tt") + "\r";
                        break;
                    default:
                        ToastPrompt toast = new ToastPrompt();
                        toast.Title = "Uh oh";
                        toast.Message = "Unable to derp";
                        toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
                        toast.MillisecondsUntilHidden = 3000;
                        toast.Show();
                        break;
                }
            }
            if (TodayDepartColumn.Text.ToString().IndexOf(":") == -1)
                ShowError(Convert.ToInt16(day));
        }


        protected override void OnNavigatedTo
        (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            String departTimes = "";
            String arriveTimes = "";
            if (NavigationContext.QueryString.TryGetValue("departTimes", out departTimes) && NavigationContext.QueryString.TryGetValue("arriveTimes", out arriveTimes))
            {
                ParseData(departTimes, arriveTimes, "0");
            }

            if (NavigationContext.QueryString.TryGetValue("line", out Line) && NavigationContext.QueryString.TryGetValue("stationFrom", out From) && NavigationContext.QueryString.TryGetValue("stationTo", out To))
            {
                LoadNormalDays();
            }
        }
    }
}