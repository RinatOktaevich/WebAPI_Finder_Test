using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("Add")]
        public async Task<HttpResponseMessage> AddTrack(string email, int idcat, string performer, string tittle)
        {
            string soundtrack;
            var user = db.Users.First(u => u.UserName == email);
            var Server = HttpContext.Current.Server;

            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
                }
                string realPath = Server.MapPath("/Data/" + user.Login + "/");
                Helper.IsUserDirectoryExist(realPath);
                Helper.CheckAudioDir(realPath);


                soundtrack = Helper.SaveImage("/Data/" + user.Login + "/Audios/");
            }
            catch (Exception)
            {

                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }


            user.AudioTracks.Add(new Audio(_url: soundtrack, _pr: performer, _ttl: tittle, authLogin: user.Login, idcat: idcat));

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<HttpResponseMessage> DeleteTrack(int idTrack)
        {
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

        [HttpPost]
        [Route("Like")]
        public async Task<HttpResponseMessage> LikeTrack(string idUser, int idTrack)
        {
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


        #region Changed
        [AllowAnonymous]
        [HttpPost]
        [Route("AudioList")]
        public IHttpActionResult GetAudios(string iduser,int categoryid)
        {
            var user = db.Users.Include(xr=>xr.AudioTracks).Where(xr=>xr.Id==iduser).ToList()[0];
            if (user == null)
            {
                return BadRequest("User doesn`t found");
            }




           var audios = user.AudioTracks.Where(xr => xr.CategoryId == categoryid).ToList();

            //foreach (var item in audios)
            //{
            //    item.Audios = user.AudioTracks.Where(xr => xr.CategoryId == item.Id).ToList();
            //}

            

          

            return Ok(audios);
        }
        #endregion

        #region Origin
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("AudioList")]
        //public IHttpActionResult GetAudios(string iduser)
        //{
        //    var user = db.Users.Find(iduser);
        //    if (user == null)
        //    {
        //        return BadRequest("User doesn`t found");
        //    }
        //    #region Changed
        //    var audios = db.Categories.ToList();

        //    foreach (var item in audios)
        //    {
        //        item.Audios = user.AudioTracks.Where(xr => xr.CategoryId == item.Id).ToList();
        //    }

        //    #endregion

        //    #region Origin
        //    //var audios = db.AudioTracks.Where(tr => tr.ApplicationUserId == iduser).ToList();
        //    #endregion

        //    return Ok(audios);
        //}
        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="iduser">Id User`s owner tracks</param>
        /// <param name="idAuth">Id User who watch a page</param>
        /// <returns>Return Audios with checked track witch was liked by idAuth user</returns>
        [HttpPost]
        [Route("AudioListAuth")]
        public IHttpActionResult GetAudios(string iduser, string idAuth)
        {
            var user = db.Users.Find(iduser);
            var auth = db.Users.Find(idAuth);
            if (user == null || auth == null)
            {
                return BadRequest("User doesn`t found");
            }

            #region Changed
            var audios = db.Categories.ToList();

            foreach (var item in audios)
            {
                item.Audios = user.AudioTracks.Where(xr => xr.CategoryId == item.Id).ToList();
            }
            //Run on category
            foreach (var cat in audios)
            {


                //Run on audios in category concrete user and stamp like if it does
                foreach (var item in cat.Audios)
                {
                    var likes = db.Likes.Where(xr => xr.AudioId == item.Id).ToList();

                    IEnumerable<object> res = (from l in likes
                                               where l.ApplicationUserId == idAuth
                                               select new { l.ApplicationUserId, l.AudioId });

                    if (res.Count() != 0)
                    {
                        item.IsLicked = true;
                    }
                    else
                    {
                        item.IsLicked = false;
                    }
                    item.Likes = null;
                }
            }
            #endregion

            #region Origin
            //var audios = user.AudioTracks.ToList();

            //foreach (var item in audios)
            //{
            //    var likes = db.Likes.Where(xr => xr.AudioId == item.Id).ToList();

            //    IEnumerable<object> res = (from l in likes
            //                               where l.ApplicationUserId == idAuth
            //                               select new { l.ApplicationUserId, l.AudioId });

            //    if (res.Count() != 0)
            //    {
            //        item.IsLicked = true;
            //    }
            //    else
            //    {
            //        item.IsLicked = false;
            //    }
            //    item.Likes = null;
            //}
            #endregion

            return Ok(audios);
        }

    }
}