using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Moslah.Models.OwnModels
{
    public class City
    {
        [Key]
        public int CityCode { get; set; }
        public string CityName { get; set; }
        public virtual ICollection<Stations> Stations { set; get; }


    }
}