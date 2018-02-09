using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI_Finder_Test.Models;

namespace WebAPI_Finder_Test.Controllers
{
    [Authorize]
    [RoutePrefix("api/Photos")]
    public class PhotoController : ApiController
    {
        /// <summary>
        /// Add photo to user collections of photos
        /// </summary>
        /// <param name="email">Email of current user</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("AddPhoto")]
        public HttpResponseMessage AddPhoto(string email)
        {
            string image;
            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
                }

                image = SaveImage("/Photos/");

            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            ApplicationDbContext db = new ApplicationDbContext();
            db.Users.First(u => u.Email == email).Photos.Add(new Photo(image));
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.Created);
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/Account/setAvatar")]
        public HttpResponseMessage InsertAvatar(string email)
        {
            string image;
            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
                }

                image = SaveImage("/Images/");
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
           
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.First(u => u.Email == email);
            if (user.AvatarImage != null)
            {
                File.Delete(HttpContext.Current.Server.MapPath(user.AvatarImage));
            }
            user.AvatarImage = image;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        #region Helpers
        /// <summary>
        /// Save uploaded image file to the server folder
        /// </summary>
        /// <returns>Return server path of file</returns>
        private string SaveImage(string ServerPath)
        {
            var file = HttpContext.Current.Request.Files[0];

            if (file.ContentLength == 0)
                throw new HttpResponseException(HttpStatusCode.NoContent);

            var name = ServerPath + Path.GetRandomFileName().Substring(0, 6) + Path.GetFileName(file.FileName);
            string filePath = HttpContext.Current.Server.MapPath(name);

            file.SaveAs(filePath);
            return name;
        }



        #endregion

    }
}