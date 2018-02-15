﻿using System;
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
            db.Users.First(u => u.UserName == email).AudioTracks.Add(new Audio(_url: soundtrack, _pr: performer, _ttl: tittle));

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

            if (Likes.Count() == 0)
            {
                db.Likes.Add(new Like(idTrack, idUser));
            }
            else
            {
                db.Likes.Remove(Likes[0]);
            }

            await db.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);
        }




    }
}