using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moslah.Models.OwnModels
{
    public class BusLocation
    {
        [Key, Column(Order = 0)]

        public int BusNumber { get; set; }
        public int PriceBus { get; set; }
        [Key, Column(Order = 1)]

        public string Source { get; set; }
        public string Destination { get; set; }
        public virtual Stations Stations { get; set; }

    }
}