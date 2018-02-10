using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace WebAPI_Finder_Test.Models.Helpers
{
    public static class FileSaver
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


    }
}