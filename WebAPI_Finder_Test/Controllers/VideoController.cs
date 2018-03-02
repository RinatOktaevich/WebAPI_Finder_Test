using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebAPI_Finder_Test.Models;
using WebAPI_Finder_Test.Models.Helpers;

namespace WebAPI_Finder_Test.Controllers
{
    [Authorize]
    [RoutePrefix("api/Videos")]
    public class VideoController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("Add")]
        public async Task<HttpResponseMessage> Add(string iduser, string name, bool iframe = false)
        {
            string vidos;
            var user = db.Users.Find(iduser);
            var Server = HttpContext.Current.Server;

            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
                }

                vidos = FileSaver.SaveImage("/Data/" + user.Login + "/Videos/");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            user.Videos.Add(new Video() { Name = name, Iframe = iframe, Url = vidos, Date = DateTime.Now });

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<HttpResponseMessage> Delete(int idvideo)
        {
            Video video;
            try
            {
                video = db.Videos.Find(idvideo);

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233079)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Video wasn`t found");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }

            File.Delete(HttpContext.Current.Server.MapPath(video.Url));
            db.Videos.Remove(video);
            await db.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("Tolist")]
        public IHttpActionResult ToList(string iduser)
        {
            var user = db.Users.Find(iduser);

            if (user == null)
            {
                return BadRequest("User doesn`t exist");
            }
           
            return Ok(db.Videos.Where(xr=>xr.ApplicationUserId==user.Id).ToList());
        }
    }
}
