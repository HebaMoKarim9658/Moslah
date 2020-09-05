using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.Web.Http;

namespace Moslah.Controllers
{
    [RoutePrefix("Moslah/City")]
    public class CityController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Route("")]
        public IHttpActionResult get()
        {
            if (db.cities.ToList().Count == 0)
                return NotFound();
            return Ok(db.cities.ToList());
        }
        [Route("CreateCity/{cy}")]
        public IHttpActionResult Post(City cy)
        {
            var res = db.cities.FirstOrDefault(m => m.CityCode == cy.CityCode);
            if (res != null)
                return NotFound();
            db.cities.Add(cy);
            db.SaveChanges();
            return Ok();
        }
        [Route("UpdateCity/{CityCode}/{cy}")]
        public IHttpActionResult Put(int CityCode, City cy)
        {
            var s = db.cities.FirstOrDefault(a => a.CityCode == CityCode);

            if (s != null)
            {
                s.CityName = cy.CityName;
                s.Stations = cy.Stations;
                db.SaveChanges();
                return Ok();
            }
            else
                return NotFound();
        }
        [Route("RemoveCity/{CityCode}")]

        public IHttpActionResult delete(int CityCode)
        {
            var st = db.cities.FirstOrDefault(a => a.CityCode == CityCode);
            if (st == null)
                return NotFound();
            db.cities.Remove(st);
            db.SaveChanges();
            return Ok();
        }
    }
}