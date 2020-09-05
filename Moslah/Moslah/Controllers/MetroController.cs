using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace Moslah.Controllers
{
    [RoutePrefix("Moslah/Metro")]
    public class MetroController : ApiController
    {
        ApplicationDbContext DB = new ApplicationDbContext();
        Dictionary<int, List<string>> MetroStaions = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> SourceLines = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> DestLines = new Dictionary<int, List<string>>();
        List<string> finalLines = new List<string>();
        List<MetroLocation> ListOfMetro = new List<MetroLocation>();

        // GET: Bus
        [Route("CityMetro/{city}")]
        public IHttpActionResult GetMetro(string city)
        {
            ListOfMetro = DB.metroLocations.Where(m=>m.Stations.City.CityName==city).ToList();
            if (ListOfMetro.Count == 0)
                return NotFound();

            return Ok(ListOfMetro);
        }
        public List<MetroLocation> GetMetroByDestination(string destination)
        {
            List<MetroLocation> metro = ListOfMetro.Where(s => s.Destination == destination).ToList();
            return metro;
        }
        public List<MetroLocation> GetMetroBySource(string Source)
        {
            List<MetroLocation> metro = ListOfMetro.Where(s => s.Source == Source).ToList();
            return metro;
        }
        [Route("MetroRoad/{Source}/{Destination}")]
        public IHttpActionResult GetMetroRoad(string Source,string Destination)
        {
            ReformateDB();
            SrcDestLines(Source, Destination);
            FindtheRoad();
            for (int i = 0; i < finalLines.Count; i++)
            {
                QuickSearch quickSearch = new QuickSearch();
                quickSearch.Destination = Destination;
                quickSearch.Source = Source;
                quickSearch.RoadDesc = finalLines[i];
                DB.quicksearch.Add(quickSearch);
            }
            if (finalLines.Count == 0)
                return NotFound();
            return Ok( finalLines);

        }
        //This Function is for reformate db and return dictionary with key equal bus number
        //Values equal the stations of the bus
        public  Dictionary<int, List<string>> ReformateDB()
        {
            int busnumOLd = ListOfMetro[0].MetroNumber;
            for (int i = 0; i < ListOfMetro.Count; i++)
            {
                int CurrBusNum = ListOfMetro[i].MetroNumber;
                if (i != ListOfMetro.Count - 1)
                {
                    //If It has the Same bus number of the previuse so it will be a station
                    //of the same bus so add it to the bus
                    if (busnumOLd == CurrBusNum)
                    {
                        var keyFounded = MetroStaions.FirstOrDefault(k => k.Key == CurrBusNum);
                        if (keyFounded.Key != 0)
                        {
                            MetroStaions[CurrBusNum].Add(ListOfMetro[i].Source);
                        }
                        else
                        {
                            MetroStaions.Add(CurrBusNum, new List<string>() { ListOfMetro[i].Source });
                        }
                    }
                    else
                    {
                        //I'll add the last station of the previous bus
                        MetroStaions[busnumOLd].Add(ListOfMetro[i - 1].Destination);
                        //then start to create a new index with new bus number for the 
                        //Current bus
                        busnumOLd = CurrBusNum;
                        var keyFounded = MetroStaions.FirstOrDefault(k => k.Key == CurrBusNum);
                        if (keyFounded.Key != 0)
                        {
                            MetroStaions[CurrBusNum].Add(ListOfMetro[i].Source);
                        }
                        else
                        {
                            MetroStaions.Add(CurrBusNum, new List<string>() { ListOfMetro[i].Source });
                        }
                    }
                }
                else
                {
                    //this is for last bus in the db 
                    MetroStaions[busnumOLd].Add(ListOfMetro[i].Source);
                    MetroStaions[busnumOLd].Add(ListOfMetro[i].Destination);
                }
            }
            return MetroStaions;

        }

        public void SrcDestLines(string Source, string Destination)
        {
            List<int> keyList = new List<int>(MetroStaions.Keys);
            List<List<string>> valueList = new List<List<string>>(MetroStaions.Values);
            for (int i = 0; i < keyList.Count; i++)
            {
                var MetroNumber = keyList[i];
                if (MetroStaions[MetroNumber].FirstOrDefault(k => k == Source) != null && MetroStaions[MetroNumber].FirstOrDefault(k => k == Destination) != null)
                {
                    string s = $"You can take {MetroNumber} bus from {Source} , it'll arrive you to {Destination} ";
                    finalLines.Add(s);
                }
                else if (MetroStaions[MetroNumber].FirstOrDefault(k => k == Source) != null)
                {
                    SourceLines.Add(MetroNumber, MetroStaions[MetroNumber]);
                }

                else if (MetroStaions[MetroNumber].FirstOrDefault(k => k == Destination) != null)
                {
                    DestLines.Add(MetroNumber, MetroStaions[MetroNumber]);
                }

            }

        }

        public void FindtheRoad()
        {
            List<List<string>> ValueListSrc = new List<List<string>>(SourceLines.Values);
            List<int> KeyListSrc = new List<int>(SourceLines.Keys);
            List<int> KeyListDest = new List<int>(DestLines.Keys);

            for (int i = 0; i < SourceLines.Count; i++)
            {
                for (int j = 0; j < DestLines.Count; j++)
                {
                    int num_source = KeyListSrc[i];
                    int num_dest = KeyListDest[j];
                    string comm = CommonStations(SourceLines[num_source], DestLines[num_dest]);
                    if (comm != null)
                    {
                        finalLines.Add($"You can take {num_source} and change in {comm} then take bus {num_dest} ");
                    }

                }
            }
        }
        public static string CommonStations(List<string> source, List<string> dest)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (dest.Contains(source[i]))
                {
                    return source[i];
                }
            }
            return null;
        }
        [Route("UpdateMetro/{metronum}/{metro}")]
        public IHttpActionResult PutMetro(int metronum, MetroLocation metro)//Add new bus
        {
            MetroLocation metroedit = DB.metroLocations.FirstOrDefault(m => m.MetroNumber == metronum);
            if (metroedit == null)
                return NotFound();
            metroedit.MetroNumber = metro.MetroNumber;
            metroedit.Destination = metro.Destination;
            metroedit.Source = metro.Source;
            metroedit.Stations = metro.Stations;
            metroedit.PriceMetro = metro.PriceMetro;
            DB.SaveChanges();
            return Ok();
        
        }
        [Route("CreateMetro/{metro}")]

        public IHttpActionResult Post(MetroLocation metro)//Add new bus
        {
            if (ListOfMetro.FirstOrDefault(m => m.MetroNumber == metro.MetroNumber) == null)
            {
                DB.metroLocations.Add(metro);
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        [Route("RemoveMetro/{metroNo}")]

        public IHttpActionResult DeleteMetro(int metroNo)// deleteByBusNo.
        {
            MetroLocation MetroLocation = ListOfMetro.FirstOrDefault(s => s.MetroNumber == metroNo);
            if(MetroLocation==null)
            {
                return NotFound();
            }
            DB.metroLocations.Remove(MetroLocation);
            DB.SaveChanges();
            return Ok();
        }



    }
}