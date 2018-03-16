using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI_Finder_Test.Models;
using WebAPI_Finder_Test.Models.DI.City_IoC;
using WebAPI_Finder_Test.Models.DI.Country_IoC;

namespace WebAPI_Finder_Test.Controllers
{
    [Authorize]
    [RoutePrefix("api/City")]
    public class CityController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ICityRepository CityRepo;
        ICountryRepository CountryRepo;


        public CityController(ICityRepository _cityrepo, ICountryRepository _countryrepo)
        {
            CityRepo = _cityrepo;
            CountryRepo = _countryrepo;
        }

        [HttpPost]
        [Route("Add")]
        public HttpResponseMessage AddCity(int countryid, string name)
        {
            var country = CountryRepo.Find(countryid);
            if (country == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Country Id doesn`t exist");
            }

            CityRepo.Add(new City() { CountryId = countryid, Name = name });
            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        [Route("Delete")]
        public HttpResponseMessage DeleteCity(int cityid)
        {
            var city = CityRepo.Find(cityid);
            if (city == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "City Id doesn`t exist");
            }

            CityRepo.Remove(city);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        [Route("Rename")]
        public HttpResponseMessage RenameCity(int cityid, string name)
        {
            var city =CityRepo.Find(cityid);
            if (city == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "City Id doesn`t exist");
            }
            city.Name = name;
            CityRepo.MarkAsModified(city);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }



        [HttpGet]
        [Route("ToList")]
        public IHttpActionResult GetCities()
        {
            var cities =  CityRepo.GetList();
            return Ok(cities); ;
        }

    }
}
