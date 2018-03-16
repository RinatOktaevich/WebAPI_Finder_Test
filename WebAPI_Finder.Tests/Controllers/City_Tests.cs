using NUnit;
using WebAPI_Finder_Test.Controllers;
using Moq;
using WebAPI_Finder_Test.Models.DI.City_IoC;
using WebAPI_Finder_Test.Models.DI.Country_IoC;
using WebAPI_Finder_Test.Models;
using System.Net.Http;
using System.Web.Http;
using WebAPI_Finder_Test;
using System.Collections.Generic;
using NUnit.Framework;
using System.Net;
using System.Web.Http.Results;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

namespace WebAPI_Finder.Tests.Controllers
{

    [TestFixture]
    public class CityControllerTest
    {
        CityController controller;
        List<City> list;

        [SetUp]
        public void Init()
        {
            list = new List<City>() { new City() { Id = 1, Name = "Odessa" }, new City() { Id = 2, Name = "Keiv" }, new City() { Id = 3, Name = "Donecck" } };
        }
        [TestCase(1, "Ukraine")]
        [Test]
        public void AddCity_Ok(int id, string countryName)
        {
            //Arange 
            var city = new City() { Id = 1, Name = "Kiev" };

            var moqcity = new Mock<ICityRepository>();
            var moqcountry = new Mock<ICountryRepository>();



            moqcountry.Setup(c => c.Find(1)).Returns(new WebAPI_Finder_Test.Models.Country() { Id = id, Name = countryName });



            controller = new CityController(moqcity.Object, moqcountry.Object);

            //Act
            HttpResponseMessage res = controller.AddCity(1, "Odessa");

            //Assert
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<HttpResponseMessage>(res);
            Assert.NotNull(list.Find(x => x.CountryId == id));
            Assert.AreEqual(res.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void AddCity_Badrequest()
        {
            //Arange 

            var moqcity = new Mock<ICityRepository>();
            var moqcountry = new Mock<ICountryRepository>();

            moqcountry.Setup(c => c.Find(1)).Returns(() => { return null; });

            controller = new CityController(moqcity.Object, moqcountry.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            HttpResponseMessage res = controller.AddCity(1, "Odessa");

            //Assert
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<HttpResponseMessage>(res);
            Assert.AreEqual(res.StatusCode, HttpStatusCode.BadRequest);
            StringAssert.Contains("Country Id doesn`t exist", res.Content.ReadAsStringAsync().Result);

        }


        [Test]
        public void GetCities_RightCollection()
        {
            //Arrange

            //var moqcity = new Mock<ICityRepository>();
            //var moqcountry = new Mock<ICountryRepository>();

            //moqcity.Setup(c =>  c.GetList()).Returns(list);

            controller = new CityController(new CityRepository(), new CountryRepository());
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();


            //Act
            OkNegotiatedContentResult<IEnumerable<City>> res = controller.GetCities() as OkNegotiatedContentResult<IEnumerable<City>>;
            var content = res.Content;



            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Content);
            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<City>>>(res);
            Assert.IsInstanceOf<IEnumerable<City>>(res.Content);
            CollectionAssert.IsEmpty(res.Content.First().Users,"City collection isn`t empty");

        }


    }
}
