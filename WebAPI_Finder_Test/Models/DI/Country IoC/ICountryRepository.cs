using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI_Finder_Test.Models.DI.Country_IoC
{
    public interface ICountryRepository
    {
        Country Find(int id);

    }
}
