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
        //|////////////////////////////////////////////////////////////////////////////////////////////////Insta
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

        //|///////////////////////////////////////////////////////////////////////////////////////////////facebook
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

        //|///////////////////////////////////////////////////////////////////////////////////////////////Youtube
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

        //|//////////////////////////////////////////////////////////////////////////////////////////////iTunes
        [HttpPost]
        [Route("Set/iTunes")]
        public async Task<HttpResponseMessage> iTunes(string iduser, string value)
        {
            iTunesLink itunes = new iTunesLink(Request);
            return await itunes.Insert(iduser, value);
        }

        [HttpPost]
        [Route("Drop/iTunes")]
        public async Task<HttpResponseMessage> iTunes(string iduser)
        {
            iTunesLink itunes = new iTunesLink(Request);
            return await itunes.Delete(iduser);
        }

        //|///////////////////////////////////////////////////////////////////////////////////////////////SoundCloud
        [HttpPost]
        [Route("Set/SoundCloud")]
        public async Task<HttpResponseMessage> SoundCloud(string iduser, string value)
        {
            SoundCloudLink soundcloud = new SoundCloudLink(Request);
            return await soundcloud.Insert(iduser, value);
        }

        [HttpPost]
        [Route("Drop/SoundCloud")]
        public async Task<HttpResponseMessage> SoundCloud(string iduser)
        {
            SoundCloudLink soundcloud = new SoundCloudLink(Request);
            return await soundcloud.Delete(iduser);
        }



    }
}
