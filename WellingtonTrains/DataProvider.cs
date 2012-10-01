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
using System.Net.NetworkInformation;
using Coding4Fun.Phone.Controls;

namespace WellingtonTrains
{
    public partial class DataProvider
    {
        const string ConnectionString = "Data Source=isostore:/TrainsDB.sdf";
        public DataProvider()
        {
            using (var db = new TrainsContext(ConnectionString))
            {
                if (!db.DatabaseExists())
                {
                    db.CreateDatabase();
                    AddData(db);
                }
            }
        }

        private void AddData(TrainsContext db)
        {
            db.Stations.InsertOnSubmit(new StationClass() { Id = 1, Name = "Wellington" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 2, Name = "Crofton Downs" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 3, Name = "Ngaio" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 4, Name = "Awarua Street" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 5, Name = "Simla Crescent" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 6, Name = "Box Hill" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 7, Name = "Khandallah" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 8, Name = "Raroa" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 9, Name = "Johnsonville" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 11, Name = "Kaiwharawhara" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 12, Name = "Ngauranga" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 13, Name = "Petone" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 14, Name = "Ava" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 15, Name = "Woburn" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 16, Name = "Waterloo" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 17, Name = "Epuni" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 18, Name = "Naenae" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 19, Name = "Wingate" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 20, Name = "Taita" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 21, Name = "Pomare" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 22, Name = "Manor Park" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 23, Name = "Silverstream" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 24, Name = "Heretaunga" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 25, Name = "Trentham" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 26, Name = "Wallaceville" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 27, Name = "Upper Hutt" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 32, Name = "Takapu Rd" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 33, Name = "Redwood" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 34, Name = "Tawa" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 35, Name = "Linden" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 36, Name = "Kenepuru" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 37, Name = "Porirua" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 38, Name = "Paremata" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 39, Name = "Mana" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 40, Name = "Plimmerton" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 41, Name = "Pukerua Bay" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 43, Name = "Paekakariki" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 44, Name = "Paraparaumu" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 72, Name = "Waikanae" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 54, Name = "Western Hutt" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 55, Name = "Melling" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 64, Name = "Maymorn" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 65, Name = "Featherston" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 66, Name = "Woodside" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 67, Name = "Matarawa" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 68, Name = "Carterton" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 69, Name = "Solway" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 70, Name = "Renall St" });
            db.Stations.InsertOnSubmit(new StationClass() { Id = 71, Name = "Masterton" });

            //db.Lines.InsertOnSubmit(new LineClass() { Id = 0, Name = "All Lines" });
            db.Lines.InsertOnSubmit(new LineClass() { Id = 1, Name = "Johnsonville" });
            db.Lines.InsertOnSubmit(new LineClass() { Id = 2, Name = "Upper Hutt" });
            db.Lines.InsertOnSubmit(new LineClass() { Id = 3, Name = "Kapiti" });
            db.Lines.InsertOnSubmit(new LineClass() { Id = 4, Name = "Melling" });
            db.Lines.InsertOnSubmit(new LineClass() { Id = 5, Name = "Wairarapa" });

            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 1, StationId = 1 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 1, StationId = 2 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 1, StationId = 3 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 1, StationId = 4 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 1, StationId = 5 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 1, StationId = 6 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 1, StationId = 7 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 1, StationId = 8 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 1, StationId = 9 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 1 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 11 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 12 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 13 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 14 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 15 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 16 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 17 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 18 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 19 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 20 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 21 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 22 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 23 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 24 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 25 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 26 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 2, StationId = 27 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 1 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 11 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 32 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 33 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 34 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 35 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 36 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 37 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 38 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 39 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 40 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 41 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 43 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 44 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 3, StationId = 72 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 4, StationId = 1 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 4, StationId = 11 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 4, StationId = 12 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 4, StationId = 13 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 4, StationId = 54 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 4, StationId = 55 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 1 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 13 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 16 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 27 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 64 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 65 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 66 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 67 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 68 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 69 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 70 });
            db.StationLines.InsertOnSubmit(new StationLineClass() { LineId = 5, StationId = 71 });

            db.SubmitChanges();
        }

        public IList<StationClass> GetStations(LineClass line, StationClass selectedStation)
        {

            List<StationClass> stations = new List<StationClass>();
            if (line.Id == 0)
            {
                using (var db = new TrainsContext(ConnectionString))
                {
                    var query = from station in db.Stations
                                orderby station.Name
                                select station;
                    stations = query.ToList();
                }
            }
            else
            {
                using (var db = new TrainsContext(ConnectionString))
                {
                    var query = from station in db.Stations
                                join stationLine in db.StationLines on station.Id equals stationLine.StationId
                                where stationLine.LineId == line.Id
                                orderby station.Id
                                select station;
                    stations = query.ToList();
                }
            }

            List<StationClass> toRemove = new List<StationClass>();
            using (var db = new TrainsContext(ConnectionString))
            {
                var query = from station in db.Stations
                            where station.Id == selectedStation.Id
                            orderby station.Name
                            select station;
                toRemove = query.ToList();
            }
            stations = new List<StationClass>(stations.Except(toRemove, new StationComparer()));
            return stations;
        }

        class StationComparer : IEqualityComparer<StationClass>
        {
            public bool Equals(StationClass x, StationClass y)
            {
                return x.Id.Equals(y.Id);
            }

            public int GetHashCode(StationClass obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        public IList<StationClass> GetStations(LineClass line)
        {
            if (line.Id == 0)
            {
                return GetStations();
            }
            else
            {
                List<StationClass> stations = new List<StationClass>();
                using (var db = new TrainsContext(ConnectionString))
                {
                    var query = from station in db.Stations
                                join stationLine in db.StationLines on station.Id equals stationLine.StationId
                                where stationLine.LineId == line.Id
                                orderby station.Id
                                select station;
                    stations = query.ToList();
                }

                return stations;
            }
        }

        public IList<StationClass> GetStations()
        {
            List<StationClass> stations = new List<StationClass>();
            using (var db = new TrainsContext(ConnectionString))
            {
                var query = from station in db.Stations
                            orderby station.Name
                            select station;
                stations = query.ToList();
            }

            return stations;
        }

        public IList<LineClass> GetLines()
        {
            List<LineClass> lines = new List<LineClass>();
            using (var db = new TrainsContext(ConnectionString))
            {
                var query = from e in db.Lines
                            select e;
                lines = query.ToList();
            }

            return lines;
        }

        public IList<StationLineClass> GetStationLines()
        {
            List<StationLineClass> stationLines = new List<StationLineClass>();
            using (var db = new TrainsContext(ConnectionString))
            {
                var query = from e in db.StationLines
                            select e;
                stationLines = query.ToList();
            }

            return stationLines;
        }

    }

    [Table]
    public class StationClass
    {
        [Column(IsDbGenerated = false, IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column()]
        public string Name { get; set; }
    }

    [Table]
    public class LineClass
    {
        [Column(IsDbGenerated = false, IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column()]
        public string Name { get; set; }
    }

    [Table]
    public class StationLineClass
    {
        [Column(IsDbGenerated = false, IsPrimaryKey = true)]
        public int StationId { get; set; }
        [Column(IsDbGenerated = false, IsPrimaryKey = true)]
        public int LineId { get; set; }
    }

    [Database]
    public class TrainsContext : DataContext
    {
        public TrainsContext(string sConnectionString)
            : base(sConnectionString)
        { }

        public Table<StationClass> Stations
        {
            get
            {
                return this.GetTable<StationClass>();
            }
        }

        public Table<LineClass> Lines
        {
            get
            {
                return this.GetTable<LineClass>();
            }
        }

        public Table<StationLineClass> StationLines
        {
            get
            {
                return this.GetTable<StationLineClass>();
            }
        }
    }
}