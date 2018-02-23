using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI_Finder_Test.Models;

namespace WebAPI_Finder_Test.Controllers
{
    [Authorize]
    [RoutePrefix("api/Country")]
    public class CountryController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("Add")]
        public async Task<HttpResponseMessage> AddCountry(string name)
        {
            db.Countries.Add(new Country() { Name = name });
            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<HttpResponseMessage> DeleteCountry(int idcountry)
        {
            var country = db.Countries.Find(idcountry);
            if (country == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            db.Countries.Remove(country);
            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }



        [HttpPost]
        [Route("Rename")]
        public async Task<HttpResponseMessage> RenameCountry(int idcountry, string name)
        {
            var country = db.Countries.Find(idcountry);
            if (country == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            country.Name = name;
            db.Entry(country).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpGet]
        [Route("ToList")]
        public IHttpActionResult Get_Countries()
        {
            var res = db.Countries.ToList();
            return Ok(res);
        }


    }
}
