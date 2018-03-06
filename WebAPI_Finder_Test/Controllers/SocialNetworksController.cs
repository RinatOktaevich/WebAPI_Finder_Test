using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI_Finder_Test.Models;
using WebAPI_Finder_Test.Models.SocialLinks;

#region Errors
//Список кодов ошибок 
//Code:1    Message:User doesn`t exict
//Code:2    Message:Url  isn`t valid
#endregion

namespace WebAPI_Finder_Test.Controllers
{
    [Authorize]
    [RoutePrefix("api/SocialNetworks")]
    public class SocialNetworksController : ApiController
    {
        /// <summary>
        /// Здесь для того чтобы установить значение нужно просто передать нужный параметр,не обязательно передавать все параметры.
        //Но что бы удалить один из параметров нужно передать значение 'null'
        /// </summary>
        //[HttpPost]
        //[Route("Set")]
        //public async Task<HttpResponseMessage> SetUrl(string iduser, string insta = "", string vk = "", string fcBook = "", string youtb = "", string twitter = "")
       // {
            //var user = db.Users.Find(iduser);

            //if (user == null)
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, "User doesn`t exict");
            //}

            //var soc = user.SocNetworks;

            //if (user.SocNetworks == null)
            //{
            //    soc = new SocialNetworks();
            //}

            //Instargram
            //if (insta != "")
            //{
            //    if (insta != "null")
            //    {
            //        Regex reg = new Regex(@"https://www.instagram.com/.{3,}");
            //        if (reg.IsMatch(insta))
            //        {
            //            soc.Instagram = insta;
            //        }
            //        else
            //        {
            //            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Instagram Url isn`t valid");
            //        }
            //    }
            //    else
            //    {
            //        soc.Instagram = null;
            //    }
            //}


            //Vk
            //if (vk != "")
            //{
            //    if (vk != "null")
            //    {
            //        Regex reg = new Regex(@"https://vk.com/.{3,}");
            //        if (reg.IsMatch(vk))
            //        {
            //            soc.Vk = vk;
            //        }
            //        else
            //        {
            //            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Vk Url isn`t valid");
            //        }
            //    }
            //    else
            //    {
            //        soc.Vk = null;
            //    }
            //}



            //facebook
            //if (fcBook != "")
            //{
            //    if (fcBook != "null")
            //    {
            //        Regex reg = new Regex(@"https://www.facebook.com/.{3,}");
            //        if (reg.IsMatch(fcBook))
            //        {
            //            soc.FaceBook = fcBook;
            //        }
            //        else
            //        {
            //            return Request.CreateResponse(HttpStatusCode.InternalServerError, "facebook Url isn`t valid");
            //        }
            //    }
            //    else
            //    {
            //        soc.FaceBook = null;
            //    }
            //}



            //YouTube
            //if (youtb != "")
            //{
            //    if (youtb != "null")
            //    {
            //        Regex reg = new Regex(@"https://www.youtube.com/channel/.{3,}");
            //        if (reg.IsMatch(youtb))
            //        {
            //            soc.YouTube = youtb;
            //        }
            //        else
            //        {
            //            return Request.CreateResponse(HttpStatusCode.InternalServerError, "YouTube Url isn`t valid");
            //        }
            //    }
            //    else
            //    {
            //        soc.YouTube = null;
            //    }
            //}



            //Twitter
            //if (twitter != "")
            //{
            //    if (twitter != "null")
            //    {
            //        Regex reg = new Regex(@"https://twitter.com/.{3,}");
            //        if (reg.IsMatch(twitter))
            //        {
            //            soc.Twitter = twitter;
            //        }
            //        else
            //        {
            //            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Twitter Url isn`t valid");
            //        }
            //    }
            //    else
            //    {
            //        soc.Twitter = null;
            //    }
            //}

            //user.SocNetworks = soc;

            //db.Entry(user).State = System.Data.Entity.EntityState.Modified;

            //await db.SaveChangesAsync();

            //return new HttpResponseMessage(HttpStatusCode.OK);
       // }



        [HttpPost]
        [Route("Set/Instagram")]
        public async Task<HttpResponseMessage> Insta(string iduser,string value)
        {
            InstagramLink insta = new InstagramLink(Request);
            return await insta.Insert(iduser, value);
        }

        [HttpPost]
        [Route("Drop/Instagram")]
        public async Task<HttpResponseMessage> Insta(string iduser)
        {
            InstagramLink insta = new InstagramLink(Request);
            return await insta.Delete(iduser);
        }



        [HttpPost]
        [Route("Set/Vk")]
        public async Task<HttpResponseMessage> Vk(string iduser, string value)
        {
            VkLink vk = new VkLink(Request);
            return await vk.Insert(iduser, value);
        }

        [HttpPost]
        [Route("Drop/Vk")]
        public async Task<HttpResponseMessage> Vk(string iduser)
        {
            InstagramLink insta = new InstagramLink(Request);
            return await insta.Delete(iduser);
        }


    }
}
