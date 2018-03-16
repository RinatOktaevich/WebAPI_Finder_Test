using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebAPI_Finder_Test.Models.DI.City_IoC
{
    public class CityRepository : ICityRepository, IDisposable
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }


        #endregion
        public async void Save()
        {
            await db.SaveChangesAsync();
        }

        public City Add(City city)
        {
            var res = db.Cities.Add(city);
            Save();
            return res;
        }

        public City Find(int id)
        {
            return db.Cities.Find(id);
        }

        public void Remove(City _city)
        {
            db.Cities.Remove(_city);
            Save();
        }

        public void MarkAsModified(City _city)
        {
            db.Entry(_city).State = EntityState.Modified;
            Save();
        }

        public IEnumerable<City> GetList()
        {
            return  db.Cities.Include("Country").ToList();
        }
    }
}