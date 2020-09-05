using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moslah.Models.OwnModels
{
    public class QuickSearch
    {
        [Key, Column(Order = 0)]
        public string Source { get; set; }
        [Key, Column(Order = 1)]
        public string Destination { get; set; }
        [Key, Column(Order = 2)]
        public string RoadDesc { get; set; }

    }
}