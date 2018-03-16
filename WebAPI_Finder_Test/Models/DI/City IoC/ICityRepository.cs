using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI_Finder_Test.Models.DI.City_IoC
{
    public interface ICityRepository
    {
        City Add(City city);
        City Find(int id);
        void Remove(City _city);
        void MarkAsModified(City _city);
        IEnumerable<City> GetList();
    }
}
