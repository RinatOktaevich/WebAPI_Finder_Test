using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI_Finder_Test.Models.DI.City_IoC;
using WebAPI_Finder_Test.Models.DI.Country_IoC;

namespace WebAPI_Finder_Test.NinjectContainer
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            //Здесь делаем привязки репозитория к реализации
            Bind<ICityRepository>().To<CityRepository>();
            Bind<ICountryRepository>().To<CountryRepository>();



        }
    }
}