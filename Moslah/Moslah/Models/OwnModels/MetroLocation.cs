using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moslah.Models.OwnModels
{
    public class MetroLocation
    {
       [Key,Column(Order =0)]
        public int MetroNumber { get; set; }
        [Key, Column(Order = 1)]
        public string Source { get; set; }
        public string Destination { get; set; }
        public int PriceMetro { get; set; }
        public virtual Stations Stations { get; set; }
    }
}