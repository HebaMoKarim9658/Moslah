using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Moslah.Controllers
{
    [RoutePrefix("Moslah/MicroBus")]
    public class MicroBusController : ApiController
    {
        ApplicationDbContext DB = new ApplicationDbContext();
        Dictionary<int, List<string>> MicrobusStations = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> SourceLines = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> DestLines = new Dictionary<int, List<string>>();
        List<string> finalLines = new List<string>();
        List<MicroBus> ListOfMicro = new List<MicroBus>();

        //Lazam fe awl 3 get na3ml check 3ala el city 3ashan magabsh el db kolha
        // GET: Bus
        [Route("CityMicroBus/{city}")]
        public IHttpActionResult GetMicroBuses(string city)
        {
            ListOfMicro = DB.microbus.Where(m=>m.Stations.City.CityName==city).ToList();
            if (ListOfMicro.Count == 0)
                return NotFound();
            return Ok(ListOfMicro);
        }
        public List<MicroBus> GetMicrbusByDestination(string destination)
        {
            List<MicroBus> buses = ListOfMicro.Where(s => s.Destination == destination).ToList();
            return buses;
        }
        public List<MicroBus> GetMicrobusBySource(string Source)
        {
            List<MicroBus> buses = ListOfMicro.Where(s => s.Source == Source).ToList();
            return buses;
        }
        [Route("MicroBusRoad/{Source}/{Destination}")]
        public IHttpActionResult GetMicroBusRoad(string Source, string Destination)
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
            return Ok(finalLines);

        }
        //This Function is for reformate db and return dictionary with key equal Microbus number
        //Values equal the stations of the Microbus
        public Dictionary<int, List<string>> ReformateDB()
        {
            int busnumOLd = ListOfMicro[0].MicroID;
            for (int i = 0; i < ListOfMicro.Count; i++)
            {
                int CurrBusNum = ListOfMicro[i].MicroID;
                if (i != ListOfMicro.Count - 1)
                {
                    //If It has the Same Microbus number of the previuse so it will be a station
                    //of the same Microbus so add it to the Microbus
                    if (busnumOLd == CurrBusNum)
                    {
                        var keyFounded = MicrobusStations.FirstOrDefault(k => k.Key == CurrBusNum);
                        if (keyFounded.Key != 0)
                        {
                            MicrobusStations[CurrBusNum].Add(ListOfMicro[i].Source);
                        }
                        else
                        {
                            MicrobusStations.Add(CurrBusNum, new List<string>() { ListOfMicro[i].Source });
                        }
                    }
                    else
                    {
                        //I'll add the last station of the previous Microbus
                        MicrobusStations[busnumOLd].Add(ListOfMicro[i - 1].Destination);
                        //then start to create a new index with new Microbus number for the 
                        //Current Microbus
                        busnumOLd = CurrBusNum;
                        var keyFounded = MicrobusStations.FirstOrDefault(k => k.Key == CurrBusNum);
                        if (keyFounded.Key != 0)
                        {
                            MicrobusStations[CurrBusNum].Add(ListOfMicro[i].Source);
                        }
                        else
                        {
                            MicrobusStations.Add(CurrBusNum, new List<string>() { ListOfMicro[i].Source });
                        }
                    }
                }
                else
                {
                    //this is for last Microbus in the db 
                    MicrobusStations[busnumOLd].Add(ListOfMicro[i].Source);
                    MicrobusStations[busnumOLd].Add(ListOfMicro[i].Destination);
                }
            }
            return MicrobusStations;

        }
        public void SrcDestLines(string Source, string Destination)
        {
            List<int> keyList = new List<int>(MicrobusStations.Keys);
            List<List<string>> valueList = new List<List<string>>(MicrobusStations.Values);
            for (int i = 0; i < keyList.Count; i++)
            {
                var MicrobusNum = keyList[i];
                if (MicrobusStations[MicrobusNum].FirstOrDefault(k => k == Source) != null && MicrobusStations[MicrobusNum].FirstOrDefault(k => k == Destination) != null)
                {
                    string s = $"You can take Microbus from {Source} , it'll arrive you to {Destination}" +
                        $" => The station of this buses are in {ListOfMicro.FirstOrDefault(m=>m.MicroID==MicrobusNum).Stations}";
                    finalLines.Add(s);
                }
                else if (MicrobusStations[MicrobusNum].FirstOrDefault(k => k == Source) != null)
                {
                    SourceLines.Add(MicrobusNum, MicrobusStations[MicrobusNum]);
                }

                else if (MicrobusStations[MicrobusNum].FirstOrDefault(k => k == Destination) != null)
                {
                    DestLines.Add(MicrobusNum, MicrobusStations[MicrobusNum]);
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
                        finalLines.Add($"You can take Microbus from {SourceLines[num_source]} " +
                            $"in station {ListOfMicro.FirstOrDefault(m => m.MicroID == num_source).Stations}" +
                            $" and change in {comm} then take Microbus to {DestLines[num_dest]}");
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

        [Route("UpdateMicroBus/{Micronum}/{Microbus}")]
        public IHttpActionResult PutMicrobus(int Micronum, MicroBus Microbus)//Add new bus
        {
            MicroBus microedit = DB.microbus.FirstOrDefault(m => m.MicroID == Micronum);
            if (microedit != null)
            {
                microedit.MicroID = Microbus.MicroID;
                microedit.Destination = Microbus.Destination;
                microedit.Source = Microbus.Source;
                microedit.Stations = Microbus.Stations;
                microedit.PriceMicro = Microbus.PriceMicro;
                DB.SaveChanges();
                return Ok();
            }

            return NotFound();
        }
        [Route("CreateMicroBus/{Microbus}")]
        public IHttpActionResult PostBus(MicroBus Microbus)//Add new Microbus
        {
            if(ListOfMicro.FirstOrDefault(m=>m.MicroID == Microbus.MicroID) == null)
            {
                DB.microbus.Add(Microbus);
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        [Route("RemoveMicroBus/{MicroNo}")]
        public IHttpActionResult DeleteBus(int MicroNo)// deleteByBusNo.
        {
            MicroBus MicroLocation = ListOfMicro.FirstOrDefault(s => s.MicroID == MicroNo);
            if (MicroLocation != null)
            {
                DB.microbus.Remove(MicroLocation);
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }



    }
}