using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_Finder_Test.Models.DI.Country_IoC
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public Country Find(int id)
        {
            return db.Countries.Find(id);
        }


        public async void Save()
        {
            await db.SaveChangesAsync();
        }
    }
}