﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebAPI_Finder_Test.Models;
using WebAPI_Finder_Test.Providers;
using WebAPI_Finder_Test.Results;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.IO;
using WebAPI_Finder_Test.Models.Helpers;
using System.Web.Http.ModelBinding;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace WebAPI_Finder_Test.Controllersз
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        #region Stuff
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();


        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }
        #endregion


        [AllowAnonymous]
        [Route("Searcher")]
        [HttpPost]
        public IHttpActionResult Find([ModelBinder]Filters filters, int offset = 1)
        {
            //offset-смещение по коллекции
            //для того чтобы взять первую двадцатку при первом запросе ,нужно передать "1"
            //потом нажав кнопку "Показать ещё" передать "2" и так далее


            //Для поиска используються такие ключи
            //cityid
            //По возросту не важно какой из параметров передаётся ,там есть значения по умолчанию
            //minAge
            //maxAge
            //categoryid-по этому ключу можно передавать несколько параметров 
            //fullname-строка поиска для имени

            offset = 20 * --offset;
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable<ApplicationUser> users = db.Users.AsNoTracking()
                .Include(xr => xr.Categories)
                .Include(xr => xr.City)
                .Include(xr => xr.City.Country);
            users = filters.Check(users).Skip(offset).Take(20).ToList();

            return Ok(users);
        }

        [HttpPost]
        [Route("ChangeFirstname")]
        public async Task<HttpResponseMessage> ChangeFirstname(string iduser, string firstname)
        {
            var user = db.Users.Find(iduser);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User doesn`t exist");
            }

            if (firstname == String.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "firstname is empty");
            }

            user.Firstname = firstname;

            user.FullName = user.Firstname.ToLower() + " " + user.Lastname.ToLower();

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("ChangeLastname")]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangeLastname(string iduser, string lastname)
        {
            var user = db.Users.Find(iduser);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User doesn`t exist");
            }

            if (lastname == String.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "firstname is empty");
            }

            user.Lastname = lastname;

            user.FullName = user.Firstname.ToLower() + " " + user.Lastname.ToLower();

            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("SetAbout")]
        [HttpPost]
        public async Task<HttpResponseMessage> SetAbout(string iduser, string about)
        {
            if (iduser == string.Empty || about == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            var user = db.Users.Find(iduser);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User doesn`t exist");
            }

            user.About = about;

            this.db.Entry(user).State = EntityState.Modified;

            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Find user by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>
        /// If user doesn`t exist you will have NotFound Request.
        /// If 'email' is null ,'NoContent' Request.
        /// If user exist 'Found' request</returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage IsUserExists(string email)
        {
            if (email == null)
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            var user = UserManager.FindByEmail(email);
            if (user == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            return new HttpResponseMessage(HttpStatusCode.Found);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<HttpResponseMessage> Delete(string email)
        {
            if (email == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.Include(xr => xr.AudioTracks).FirstOrDefault(u => u.UserName == email);

            if (user == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var Server = HttpContext.Current.Server;

            db.AudioTracks.RemoveRange(user.AudioTracks);

            //Delete all user`s likes he ever liked
            var userLikes = db.Likes.Where(xr => xr.ApplicationUserId == user.Id);
            db.Likes.RemoveRange(userLikes);

            db.Users.Remove(user);

            var userDir = "/Data/" + user.Login;
            Directory.Delete(Server.MapPath(userDir), true);

            await db.SaveChangesAsync();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("get/email")]
        public IHttpActionResult Get_email(string email)
        {
            if (email == null)
                return BadRequest("Emaill is null");

            // var user = UserManager.FindByEmail(email);
            // ApplicationDbContext db = new ApplicationDbContext();

            db.Configuration.ProxyCreationEnabled = false;

            var user = db.Users.AsNoTracking()
                .Include(xr => xr.Categories)
                .Include(xr => xr.City)
                .Include(xr => xr.City.Country)
                .Include(xr => xr.SocNetworks)
                .Where(u => u.UserName == email).ToList()[0];

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Get user by login 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("get/login")]
        public IHttpActionResult Get_login(string login)
        {
            if (login == null)
                return BadRequest("Login is null");

            db.Configuration.ProxyCreationEnabled = false;

            var user = db.Users.AsNoTracking()
                .Include(xr => xr.Categories)
                .Include(xr => xr.City)
                .Include(xr => xr.City.Country)
                .Include(xr => xr.SocNetworks)
                .Where(u => u.Login == login).ToList()[0];



            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAll()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var users = db.Users.AsNoTracking()
                .Include(xr => xr.Categories)
                .Include(xr => xr.City)
                .Include(xr => xr.City.Country)
                .Include(xr => xr.SocNetworks)
                .ToList();

            return Ok(users);
        }

        [HttpPost]
        [Route("setAvatar")]
        public HttpResponseMessage InsertAvatar(string email)
        {
            //Url that we save in db
            string image;
            var user = db.Users.First(u => u.UserName == email);
            var Server = HttpContext.Current.Server;
            AzureHelper azure = new AzureHelper("data");

            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
                }


                #region Azure Image saver
                var file = HttpContext.Current.Request.Files[0];

                if (file.ContentLength == 0)
                {
                    throw new HttpResponseException(HttpStatusCode.NoContent);
                }


                #region Native work with azure blob storage
                //// Retrieve storage account information from connection string
                //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

                //// Create a blob client for interacting with the blob service.
                //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                //// Create a container for organizing blobs within the storage account.
                ////1. Creating Container
                //container = blobClient.GetContainerReference("data");

                //try
                //{
                //    BlobRequestOptions requestOptions = new BlobRequestOptions() { RetryPolicy = new NoRetry() };
                //    container.CreateIfNotExists(requestOptions, null);
                //}
                //catch (StorageException)
                //{
                //    throw;
                //}
                #endregion
                var newFileName = Path.GetRandomFileName().Substring(0, 6) + Path.GetFileName(file.FileName);
                var blobName = user.Login + "/" + newFileName;
                image = azure.UploudToContainer(blobName, file);

                #region 2 Native work with azure blob storage
                // Upload a BlockBlob to the newly created container
                //2. Uploading BlockBlob 
                //CloudBlockBlob blockBlob = container.GetBlockBlobReference(azurePath + user.Login + "/" + newFileName);
                //blockBlob.Properties.ContentType = file.ContentType;
                //blockBlob.UploadFromStream(file.InputStream);
                //image = blockBlob.Uri.AbsoluteUri;


                //CloudBlobDirectory virtualPath = container.GetDirectoryReference(azurePath + user.Login);
                //CloudBlockBlob oldBlockBlob = virtualPath.GetBlockBlobReference(avatarname);
                //oldBlockBlob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                #endregion

                #endregion
                #region Server image saver
                //string virtualPath = "/Data/" + user.Login + "/";
                //string realPath = Server.MapPath("/Data/" + user.Login + "/");

                //Helper.IsUserDirectoryExist(realPath);

                //This check and create ,if doesn`t exist ,required directories
                //  Helper.CheckAudioDir(user.Login,Server);

                //string realPath =Server.MapPath("/Data/" + user.Login+"/");
                //if(!Directory.Exists(realPath))
                //{
                //    Directory.CreateDirectory(realPath);
                //}
                //if(!Directory.Exists(realPath+"Audios"))
                //{
                //    Directory.CreateDirectory(realPath + "Audios");
                //}
                //var info = Directory.CreateDirectory(realPath);
                //var info2 = Directory.CreateDirectory(realPath + "Audios");
                //var info3 = Directory.CreateDirectory(realPath + "Videos");

                //Helper.SaveImage(virtualPath);
                #endregion
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            if (user.AvatarImage != null)
            {
                string avatarname = Path.GetFileName(user.AvatarImage);
                azure.DeleteFromContainer(user.Login, avatarname);

                #region Native work with azure blob storage
                //string avatarname = Path.GetFileName(user.AvatarImage);
                //CloudBlobDirectory virtualPath = container.GetDirectoryReference(azurePath + user.Login);
                //CloudBlockBlob oldBlockBlob = virtualPath.GetBlockBlobReference(avatarname);
                //oldBlockBlob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                #endregion
                //File.Delete(HttpContext.Current.Server.MapPath(user.AvatarImage));
            }
            user.AvatarImage = image;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("setPhoneNumber")]
        public async  Task<HttpResponseMessage> InsertPhoneNumber(string iduser, string phoneNumber)
        {
            var user = db.Users.Find(iduser);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Regex reg = new Regex(@"\d{12}",RegexOptions.Compiled);
            if (!reg.IsMatch(phoneNumber))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Incorrect phone number format");
            }

            user.PhoneNumber = phoneNumber;
            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("dropPhoneNumber")]
        public async Task<HttpResponseMessage> DeletePhoneNumber(string iduser)
        {
            var user = db.Users.Find(iduser);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            user.PhoneNumber = null;
            await db.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }



        #region Project Stuff
        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string defaultImgLink = "https://dataskitel.blob.core.windows.net/data/C:/Users/Ринат/documents/visual studio 2015/Projects/CopyDataSkitelDBToAzure/CopyDataSkitelDBToAzure/Images/defaultImg.jpg";

            ApplicationUser user = new ApplicationUser() { UserName = model.Email, Email = model.Email, Firstname = model.Firstname, Lastname = model.Lastname, BirthDate = model.BirthDate, AvatarImage = defaultImgLink, RegistrationDate = DateTime.Now, FullName = model.Firstname.ToLower() + " " + model.Lastname.ToLower(), CityId = model.CityId };

            //default avatar=/Images/defaultImg.jpg

            IdentityResult result = null;
            try
            {

                result = await UserManager.CreateAsync(user, model.Password);
            }
            catch (Exception)
            {

                return GetErrorResult(result);
            }


            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            user.Login = CreateLogin(model.Email, user.Id);

            #region Creating directories for file system
            //string userDirectory = HttpContext.Current.Server.MapPath("/Data/" + user.Login + "/");

            //var info = Directory.CreateDirectory(userDirectory);

            //var info2 = Directory.CreateDirectory(userDirectory + "Audios");
            //var info3 = Directory.CreateDirectory(userDirectory + "Videos");
            #endregion
            UserManager.Update(user);

            return Ok();
        }





        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }



        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
        #endregion

        #region Helpers


        private string CreateLogin(string mail, string ID)
        {
            Regex regex = new Regex("(?<name>.*)(@)(.*)");
            var match = regex.Match(mail);

            return match.Groups["name"].Value + ID.Substring(0, 4);
        }


        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
