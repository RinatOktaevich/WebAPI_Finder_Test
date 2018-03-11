using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using WebAPI_Finder_Test.Models;

namespace WebAPI_Finder_Test.Providers
{
    [ModelBinder(typeof(FilterModelBinder))]
    public abstract class Filters
    {
        protected Filters NextChain;

        public Filters()
        {

        }

        public Filters Chain
        {
            set
            {
                NextChain = value;
            }
        }

        public abstract IEnumerable<ApplicationUser> Check(IEnumerable<ApplicationUser> _users);

    }

    public class CityFilter : Filters
    {
        int Value;

        public CityFilter(int _value)
        {
            Value = _value;
        }

        public CityFilter()
        {

        }

        public override IEnumerable<ApplicationUser> Check(IEnumerable<ApplicationUser> _users)
        {

            IEnumerable<ApplicationUser> users = _users.Where(xr => xr.CityId == Value);

            //преходим к следующему звену
            if (NextChain != null)
            {
                return NextChain.Check(users);
            }
            return users;
        }
    }


    public class AgeFilter : Filters
    {
        int minAge;
        int maxAge;
        DateTime Now = DateTime.Now;

        public AgeFilter(int _minAge, int _maxAge)
        {
            minAge = _minAge;
            maxAge = _maxAge;
        }
        public override IEnumerable<ApplicationUser> Check(IEnumerable<ApplicationUser> _users)
        {
            IEnumerable<ApplicationUser> users = _users.Where(xr => (Now - xr.BirthDate).Days / 365 >= minAge).Where(xr => (Now - xr.BirthDate).Days / 365 <= maxAge);

            //преходим к следующему звену
            if (NextChain != null)
            {
                return NextChain.Check(users);
            }
            return users;
        }
    }

    public class CategoryFilter : Filters
    {
        string[] categories;

        public string[] Categories {
            set
            {
                categories = value;
            }
        }


        public override IEnumerable<ApplicationUser> Check(IEnumerable<ApplicationUser> _users)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<ApplicationUser> users =db.Database.SqlQuery<ApplicationUser>("Select * from AspNetUsers where ");

            //преходим к следующему звену
            if (NextChain != null)
            {
                return NextChain.Check(users);
            }
            return users;
        }
    }






}