using System;
using NUnit.Framework;
using WebAPI_Finder_Test.Controllersз;
using System.Net.Http;
using System.Web.Http;
using WebAPI_Finder_Test.Models;
using System.Web.Http.Results;

namespace WebAPI_Finder.Tests.Controllers
{
    [TestFixture]
    public class AccountController_Tests
    {
        AccountController controller;

        public AccountController_Tests()
        {
            controller = new AccountController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }

        //Integration tests
        #region Check correct data return

        //[TestCase("askdaks", false)]
        [TestCase("rd8200d")]
        [Test]
        public void Get_login_Test(string login)
        {
            OkNegotiatedContentResult<ApplicationUser> res = controller.Get_login(login) as OkNegotiatedContentResult<ApplicationUser>;

            var user = res.Content;

            Assert.IsNotNull(res);
            Assert.IsNotNull(user);
            Assert.IsInstanceOf<ApplicationUser>(user);
            CollectionAssert.IsEmpty(user.AudioTracks,"Audios must be empty");
            CollectionAssert.IsEmpty(user.Videos,"Videos must be empty");
            CollectionAssert.IsEmpty(user.City.Country.Cities,"Cities in country in city must be empty");
            CollectionAssert.IsEmpty(user.City.Users,"Users in city must be empty");

            CollectionAssert.IsNotEmpty(user.Categories);

        }



        #endregion

    }
}
