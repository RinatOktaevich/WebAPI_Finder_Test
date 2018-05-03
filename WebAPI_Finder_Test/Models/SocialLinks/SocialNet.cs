using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WebAPI_Finder_Test.Models.SocialLinks
{
    //Список кодов ошибок 
    //Code:1    Message:User doesn`t exict
    //Code:2    Message:Url  isn`t valid

    #region Deleted
    //public class VkLink : SocialNet
    //{
    //    public VkLink(HttpRequestMessage _request) : base(_request)
    //    {
    //    }

    //    protected override void DeleteValue()
    //    {
    //        soc.Vk = null;
    //    }

    //    protected override HttpResponseMessage SetValue(string value)
    //    {
    //        Regex reg = new Regex(@"https://vk.com/.{3,}");
    //        if (reg.IsMatch(value))
    //        {
    //            soc.Vk = value;
    //        }
    //        else
    //        {
    //            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Code = 2, Message = "Vk Url isn`t valid" });
    //        }

    //        return new HttpResponseMessage(HttpStatusCode.OK);
    //    }
    //}

    //public class TwitterLink : SocialNet
    //{
    //    public TwitterLink(HttpRequestMessage _request) : base(_request)
    //    {
    //    }

    //    protected override void DeleteValue()
    //    {
    //        soc.Twitter = null;
    //    }

    //    protected override HttpResponseMessage SetValue(string value)
    //    {
    //        Regex reg = new Regex(@"https://twitter.com/.{3,}");
    //        if (reg.IsMatch(value))
    //        {
    //            soc.Twitter = value;
    //        }
    //        else
    //        {
    //            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Code = 2, Message = "Twitter Url isn`t valid" });
    //        }

    //        return new HttpResponseMessage(HttpStatusCode.OK);
    //    }

    //}

   

    #endregion

    public abstract class SocialNet
    {
        ApplicationUser user;
        ApplicationDbContext db = new ApplicationDbContext();
        HttpResponseMessage Response;

        protected HttpRequestMessage Request;
        protected SocialNetworks soc;

        public SocialNet(HttpRequestMessage _request)
        {
            Request = _request;
        }

        public async Task<HttpResponseMessage> Insert(string iduser, string value)
        {
            var resp = Checks(iduser);
            if (resp.StatusCode != HttpStatusCode.OK)
                return resp;

            resp = SetValue(value);
            if (resp.StatusCode != HttpStatusCode.OK)
                return resp;

            return await SaveChanges();
        }

        public async Task<HttpResponseMessage> Delete(string iduser)
        {
            var resp = Checks(iduser);
            if (resp.StatusCode != HttpStatusCode.OK)
                return resp;
            DeleteValue();
            return await SaveChanges();
        }

        //Удаление ссылки
        protected abstract void DeleteValue();
        //Установить ссылку
        protected abstract HttpResponseMessage SetValue(string value);


        //Предварительная проверка на различные данные
        protected HttpResponseMessage Checks(string iduser)
        {
            user = db.Users.Find(iduser);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Code = 1, Message = "User doesn`t exict" });
            }

            soc = user.SocNetworks;

            if (user.SocNetworks == null)
            {
                soc = new SocialNetworks();
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        //Сохранение результата
        protected async Task<HttpResponseMessage> SaveChanges()
        {
            user.SocNetworks = soc;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }


    public class InstagramLink : SocialNet
    {
        public InstagramLink(HttpRequestMessage _request) : base(_request)
        {
        }

        protected override void DeleteValue()
        {
            soc.Instagram = null;
        }

        protected override HttpResponseMessage SetValue(string value)
        {

            Regex reg = new Regex(@"https://www.instagram.com/.{3,}");
            if (reg.IsMatch(value))
            {
                soc.Instagram = value;
            }
            else
            {
                //Code 2
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Code = 2, Message = "Instagram Url isn`t valid" });
            }

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }

    public class FacebookLink : SocialNet
    {
        public FacebookLink(HttpRequestMessage _request) : base(_request)
        {
        }

        protected override void DeleteValue()
        {
            soc.FaceBook = null;
        }

        protected override HttpResponseMessage SetValue(string value)
        {
            Regex reg = new Regex(@"https://www.facebook.com/.{3,}");
            if (reg.IsMatch(value))
            {
                soc.FaceBook = value;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Code = 2, Message = "Facebook Url isn`t valid" });
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

    public class YouTubeLink : SocialNet
    {
        public YouTubeLink(HttpRequestMessage _request) : base(_request)
        {
        }

        protected override void DeleteValue()
        {
            soc.YouTube = null;
        }

        protected override HttpResponseMessage SetValue(string value)
        {
            Regex reg = new Regex(@"https://www.youtube.com/channel/.{3,}");
            if (reg.IsMatch(value))
            {
                soc.YouTube = value;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Code = 2, Message = "YouTube Url isn`t valid" });
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }


    //iTunes
    public class iTunesLink : SocialNet
    {
        public iTunesLink(HttpRequestMessage _request) : base(_request)
        {
        }

        protected override void DeleteValue()
        {
            soc.iTunes = null;
        }

        protected override HttpResponseMessage SetValue(string value)
        {
            Regex reg = new Regex(@"https://itunes.apple.com/.{3,}");
            if (reg.IsMatch(value))
            {
                soc.iTunes = value;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Code = 2, Message = "iTunes Url isn`t valid" });
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }


    //SoundCloud
    public class SoundCloudLink : SocialNet
    {
        public SoundCloudLink(HttpRequestMessage _request) : base(_request)
        {
        }

        protected override void DeleteValue()
        {
            soc.SoundCloud = null;
        }

        protected override HttpResponseMessage SetValue(string value)
        {
            Regex reg = new Regex(@"https://soundcloud.com/.{3,}");
            if (reg.IsMatch(value))
            {
                soc.SoundCloud = value;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Code = 2, Message = "SoundCloud Url isn`t valid" });
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }


}