using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moslah.Models.OwnModels
{
    public class Description
    {
        Description()
        {
            Deleted = false;
        }
        [Key]
        public int ID { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string StatusRoad { get; set; }
        public DateTime Date { get; set; }
        public bool Deleted { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}