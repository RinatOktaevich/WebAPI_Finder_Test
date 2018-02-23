using System;
using System.Collections.Generic;
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
        public async  Task<HttpResponseMessage> AddCountry(string name)
        {
            db.Countries.Add(new Country() { Name = name });
            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
