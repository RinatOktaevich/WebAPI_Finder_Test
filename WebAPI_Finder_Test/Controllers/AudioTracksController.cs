using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
    [RoutePrefix("api/Tracks")]
    public class AudioTracksController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("Add")]
        public async Task<HttpResponseMessage> AddTrack(string email, string performer, string tittle)
        {

            string soundtrack;
            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
                }

                soundtrack = FileSaver.SaveImage("/Audio/");
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.First(u => u.UserName == email);

            user.AudioTracks.Add(new Audio(_url: soundtrack, _pr: performer, _ttl: tittle, authLogin: user.Login));

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Delete")]
        public async Task<HttpResponseMessage> DeleteTrack(int idTrack)
        {

            ApplicationDbContext db = new ApplicationDbContext();
            Audio track;
            try
            {
                track = db.AudioTracks.Find(idTrack);

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233079)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Track wasn`t found");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }

            File.Delete(HttpContext.Current.Server.MapPath(track.Url));
            db.AudioTracks.Remove(track);
            await db.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("Like")]
        public async Task<HttpResponseMessage> LikeTrack(string idUser, int idTrack)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Like like = new Like(idTrack, idUser);

            var Likes = db.Likes.Where(lk => lk.AudioId == idTrack && lk.ApplicationUserId == idUser).ToList();
            var track = db.AudioTracks.Find(idTrack);

            if (Likes.Count() == 0)
            {
                db.Likes.Add(new Like(idTrack, idUser));
                track.CountLikes += 1;
            }
            else
            {
                db.Likes.Remove(Likes[0]);
                track.CountLikes -= 1;
            }

            await db.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("AudioList")]
        public IHttpActionResult GetAudios(string iduser)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.Find(iduser);
            if (user == null)
            {
                return BadRequest("User doesn`t found");
            }

            var audios = db.AudioTracks.Where(tr => tr.ApplicationUserId == iduser).ToList();




            return Ok(audios);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AudioListAuth")]
        public IHttpActionResult GetAudios(string iduser, string idAuth)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.Find(iduser);
            var auth = db.Users.Find(idAuth);
            if (user == null || auth == null)
            {
                return BadRequest("User doesn`t found");
            }

            var audios = user.AudioTracks.ToList();    //Select(x=>new {x.ApplicationUserId,x.AuthorLogin,x.CountLikes,x.Description,x.Id,x.ImageCover,x.IsLicked,x.Performer,x.Title,x.Url });

            foreach (var item in audios)
            {
                var likes = db.Likes.Where(xr => xr.AudioId == item.Id).ToList();

                IEnumerable<object> res = (from l in likes
                                        where l.ApplicationUserId == idAuth
                                        select new { l.ApplicationUserId, l.AudioId });

                if (res.Count()!= 0)
                {
                    item.IsLiked = true;
                }
                else
                {
                    item.IsLiked = false;
                }
                item.Likes = null;
            }

            return Ok(audios);
        }


    }
}