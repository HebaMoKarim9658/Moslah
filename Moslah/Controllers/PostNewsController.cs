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
    [RoutePrefix("Moslah/RoadNews")]
    public class PostNewsController : ApiController
    {
        ApplicationDbContext DB = new ApplicationDbContext();
        [Route("")]
        public IHttpActionResult GetAllNews()
        {
            if (DB.news.OrderByDescending(o => o.Date).ToList().Count == 0)
                return NotFound();
           return Ok(DB.news.OrderByDescending(o=>o.Date).ToList());
        }
        [Route("{Source}/{Destination}")]

        public IHttpActionResult GetRelatedNews(string Source,string Destination)
        {
            if(DB.news.Where(o => o.Source == Source && o.Destination == Destination).OrderByDescending(o => o.Date).ToList().Count == 0)
                return NotFound();
            return Ok(DB.news.Where(o=>o.Source==Source&&o.Destination==Destination).OrderByDescending(o => o.Date).ToList());
        }
        [Route("CreateNews/{n}")]

        public void PostNews(News n)
        {
            DB.news.Add(n);
            DB.SaveChanges();
        }
        [Route("UpdateNews/{ID}/{NewNews}")]

        public IHttpActionResult PutNews(int ID,News NewNews)
        {
          var Old=DB.news.FirstOrDefault(o=>o.ID==ID);
            if (Old != null)
            {
                Old.Destination = NewNews.Destination;
                Old.Source = NewNews.Source;
                Old.StatusRoad = NewNews.StatusRoad;
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [Route("RemoveNews/{ID}")]
        public IHttpActionResult DeleteNews(int ID)
        {
            var news = DB.news.FirstOrDefault(o => o.ID == ID);
            if (news != null)
            {
                news.Deleted = true;
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
            
        }

    }
}
