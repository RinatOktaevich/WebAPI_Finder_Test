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
    [RoutePrefix("api/City")]
    public class CityController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        [HttpPost]
        [Route("Add")]
        public async Task<HttpResponseMessage> AddCity(int countryid, string name)
        {
            var country = db.Countries.Find(countryid);
            if (country == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Country Id doesn`t exist");
            }

            db.Cities.Add(new City() { CountryId = countryid, Name = name });

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public async Task<HttpResponseMessage> DeleteCity(int cityid)
        {
            var city = db.Cities.Find(cityid);
            if (city == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "City Id doesn`t exist");
            }

            db.Cities.Remove(city);

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPost]
        [Route("Rename")]
        public async Task<HttpResponseMessage> RenameCity(int cityid, string name)
        {
            var city = db.Cities.Find(cityid);
            if (city == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "City Id doesn`t exist");
            }

            city.Name = name;

            db.Entry(city).State = EntityState.Modified;

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("CitiesInCountry/ToList")]
        public IHttpActionResult Cities_ByCountry(int countryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cities = db.Cities.AsNoTracking().Where(xr => xr.CountryId == countryId).ToList();

            return Ok(cities);
        }


        [HttpGet]
        [Route("ToList")]
        public async Task<IHttpActionResult> GetCities()
        {
            var cities = await db.Cities.Include("Country").ToListAsync();
            return Ok(cities); ;
        }

    }
}
