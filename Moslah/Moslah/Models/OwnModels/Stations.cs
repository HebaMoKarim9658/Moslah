using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moslah.Models.OwnModels
{
    public class Stations
    {
        [Key]
        public int ID { get; set; }
        public string Type { get; set; }
        [ForeignKey("BusLocation")]
        public int? BusNumber { get; set; }
        public virtual ICollection<BusLocation> BusLocation { set; get; }
        [ForeignKey("MetroLocation")]
        public int? MetroNumber { get; set; }
        public virtual ICollection<MetroLocation> MetroLocation { set; get; }
        public virtual City City { set; get; }
        public string Name { get; set; }
        public string zone { get; set; }

        [ForeignKey("MicroBus")]
        public int? MicroID { get; set; }
        public virtual ICollection<MicroBus> MicroBus  { set; get; }


    }
}