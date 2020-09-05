using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Moslah.Controllers
{
    [RoutePrefix("Moslah/RoadDescription")]
    public class PostDescriptionController : ApiController
    {
        ApplicationDbContext DB = new ApplicationDbContext();
        [Route("")]
        public IHttpActionResult GetAllDescription()
        {
            if (DB.description.OrderByDescending(o => o.Date).ToList() == null)
                return NotFound();
            return Ok(DB.description.OrderByDescending(o => o.Date).ToList());
        }
        [Route("{Source}/{Destination}")]

        public IHttpActionResult GetRelatedDescription(string Source, string Destination)
        {
            if (DB.description.Where(o => o.Source == Source && o.Destination == Destination).OrderByDescending(o => o.Date).ToList() == null)
                return NotFound();
            return Ok(DB.description.Where(o => o.Source == Source && o.Destination == Destination).OrderByDescending(o => o.Date).ToList());
        }
        [Route("CreateDescription/{NewDescrip}")]

        public void PostDescription(Description NewDescrip)
        {
            DB.description.Add(NewDescrip);
            DB.SaveChanges();
        }

        [Route("UpdateDescription/{ID}/{NewDescription}")]
        public IHttpActionResult PutDescription(int ID, Description NewDescription)
        {
            var Old = DB.description.FirstOrDefault(o => o.ID == ID);
            if (Old != null) {
                Old.Destination = NewDescription.Destination;
                Old.Source = NewDescription.Source;
                Old.StatusRoad = NewDescription.StatusRoad;
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [Route("RemoveDescription/{ID}")]
        public IHttpActionResult DeleteDescription(int ID)
        {
            var Description = DB.description.FirstOrDefault(o => o.ID == ID);
            if (Description != null)
            {
                Description.Deleted = true;
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

    }
}

