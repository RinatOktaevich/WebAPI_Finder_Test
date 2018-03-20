using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace WebAPI_Finder_Test.Models.Helpers
{
    public static class Helper
    {
        /// <summary>
        /// Save uploaded file to the server folder
        /// </summary>
        /// <returns>Return server path of file</returns>
        public static string SaveImage(string ServerPath)
        {
            var file = HttpContext.Current.Request.Files[0];


            if (file.ContentLength == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }


            var name = ServerPath + Path.GetRandomFileName().Substring(0, 6) + Path.GetFileName(file.FileName);
            string filePath = HttpContext.Current.Server.MapPath(name);

            file.SaveAs(filePath);
            return name;
        }

        /// <summary>
        /// Check if User/Audios directory exist.If it don`t ,create.
        /// Also check for UserLogin directory
        /// </summary>
        /// <param name="Login">User Login </param>
        /// <param name="Server">Server util</param>
        public static void CheckAudioDir(string realPath)
        {
            // string realPath = Server.MapPath("/Data/" + Login + "/");

            //IsUserDirectoryExist(realPath);

            if (!Directory.Exists(realPath + "Audios"))
            {
                Directory.CreateDirectory(realPath + "Audios");
            }
        }


        /// <summary>
        /// Check if User/Audios directory exist.If it don`t ,create.
        /// Also check for UserLogin directory
        /// </summary>
        /// <param name="Login">User Login </param>
        /// <param name="Server">Server util</param>
        public static void CheckVideosDir(string realPath)
        {
            // string realPath = Server.MapPath("/Data/" + Login + "/");

           // IsUserDirectoryExist(realPath);

            if (!Directory.Exists(realPath + "Videos"))
            {
                Directory.CreateDirectory(realPath + "Videos");
            }
        }


        public static void IsUserDirectoryExist(string realPath)
        {
            if (!Directory.Exists(realPath))
            {
                Directory.CreateDirectory(realPath);
            }
        }




    }
}