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

        public async Task<HttpResponseMessage> Algorythm(string iduser, string value)
        {
            var resp = Checks(iduser);
            if (resp.StatusCode != HttpStatusCode.OK)
                return resp;

            resp = SetValue(value);
            if (resp.StatusCode != HttpStatusCode.OK)
                return resp;


            return await SaveChanges();

        }

        //Предварительная проверка на различные данные
        public HttpResponseMessage Checks(string iduser)
        {
            user = db.Users.Find(iduser);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Code = 1, Message = "you asshole" });  // Request.CreateResponse(HttpStatusCode.BadRequest, "User doesn`t exict");
            }

            soc = user.SocNetworks;

            if (user.SocNetworks == null)
            {
                soc = new SocialNetworks();
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //Сохранение результата
        public async Task<HttpResponseMessage> SaveChanges()
        {
            user.SocNetworks = soc;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public abstract HttpResponseMessage SetValue(string value);

    }


    public class InstagramLink : SocialNet
    {
        public InstagramLink(HttpRequestMessage _request) : base(_request)
        {
        }

        public override HttpResponseMessage SetValue(string value)
        {
            if (value != "")
            {
                Regex reg = new Regex(@"https://www.instagram.com/.{3,}");
                if (reg.IsMatch(value))
                {
                    soc.Instagram = value;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Instagram Url isn`t valid");
                }
            }
            else
            {
                soc.Instagram = null;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }

    public class VkLink : SocialNet
    {
        public VkLink(HttpRequestMessage _request) : base(_request)
        {
        }

        public override HttpResponseMessage SetValue(string value)
        {
            if (value != "")
            {
                Regex reg = new Regex(@"https://vk.com/.{3,}");
                if (reg.IsMatch(value))
                {
                    soc.Vk = value;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Vk Url isn`t valid");
                }
            }
            else
            {
                soc.Vk = null;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

    public class facebookLink : SocialNet
    {
        public facebookLink(HttpRequestMessage _request) : base(_request)
        {
        }

        public override HttpResponseMessage SetValue(string value)
        {
            if (value != "")
            {
                Regex reg = new Regex(@"https://www.facebook.com/.{3,}");
                if (reg.IsMatch(value))
                {
                    soc.FaceBook = value;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "facebook Url isn`t valid");
                }
            }
            else
            {
                soc.FaceBook = null;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }

    public class YouTubeLink : SocialNet
    {
        public YouTubeLink(HttpRequestMessage _request) : base(_request)
        {
        }

        public override HttpResponseMessage SetValue(string value)
        {
            if (value != "")
            {
                Regex reg = new Regex(@"https://www.youtube.com/channel/.{3,}");
                if (reg.IsMatch(value))
                {
                    soc.YouTube = value;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "YouTube Url isn`t valid");
                }
            }
            else
            {
                soc.YouTube = null;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

    public class Twitter : SocialNet
    {
        public Twitter(HttpRequestMessage _request) : base(_request)
        {
        }

        public override HttpResponseMessage SetValue(string value)
        {
            if (value != "")
            {
                Regex reg = new Regex(@"https://twitter.com/.{3,}");
                if (reg.IsMatch(value))
                {
                    soc.Twitter = value;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Twitter Url isn`t valid");
                }
            }
            else
            {
                soc.Twitter = null;
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }


}