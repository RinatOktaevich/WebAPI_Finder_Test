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
        [HttpPost]
        [Route("Set/Instagram")]
        public async Task<HttpResponseMessage> Insta(string iduser, string value)
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
            VkLink vk = new VkLink(Request);
            return await vk.Delete(iduser);
        }



        [HttpPost]
        [Route("Set/Facebook")]
        public async Task<HttpResponseMessage> Facebook(string iduser, string value)
        {
            FacebookLink fc = new FacebookLink(Request);
            return await fc.Insert(iduser, value);
        }

        [HttpPost]
        [Route("Drop/Facebook")]
        public async Task<HttpResponseMessage> Facebook(string iduser)
        {
            FacebookLink fc = new FacebookLink(Request);
            return await fc.Delete(iduser);
        }



        [HttpPost]
        [Route("Set/YouTube")]
        public async Task<HttpResponseMessage> YouTube(string iduser, string value)
        {
            YouTubeLink youtube = new YouTubeLink(Request);
            return await youtube.Insert(iduser, value);
        }

        [HttpPost]
        [Route("Drop/YouTube")]
        public async Task<HttpResponseMessage> YouTube(string iduser)
        {
            YouTubeLink youtube = new YouTubeLink(Request);
            return await youtube.Delete(iduser);
        }

        

        [HttpPost]
        [Route("Set/Twitter")]
        public async Task<HttpResponseMessage> Twitter(string iduser, string value)
        {
            TwitterLink twitter = new TwitterLink(Request);
            return await twitter.Insert(iduser, value);
        }

        [HttpPost]
        [Route("Drop/Twitter")]
        public async Task<HttpResponseMessage> Twitter(string iduser)
        {
            TwitterLink twitter = new TwitterLink(Request);
            return await twitter.Delete(iduser);
        }




    }
}
