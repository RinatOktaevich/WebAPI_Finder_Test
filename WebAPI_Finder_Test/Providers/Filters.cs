using System;
using System.Collections;
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
        List<string> categories = new List<string>();

        public CategoryFilter(ref string[] cats)
        {
            categories.AddRange(cats);
        }


        public override IEnumerable<ApplicationUser> Check(IEnumerable<ApplicationUser> _users)
        {
           // Здесь физически все юзеры
            _users = _users.ToList();
            Category tmp = new Category();

           // Result collection
            ICollection<ApplicationUser> Users = new List<ApplicationUser>();


            foreach (var UserItem in _users)
            {
                foreach (var CatItem in categories)
                {
                    var id = Convert.ToInt16(CatItem);
                    var cat = UserItem.Categories.Any(xr => xr.Id == id);
                    if (cat == true)
                    {
                        Users.Add(UserItem);
                        break;
                    }
                }
            }

            //преходим к следующему звену
            if (NextChain != null)
            {
                return NextChain.Check(Users);
            }
            return Users;
        }
    }


    public class FullNameFilter : Filters
    {
        string[] Values;

        public FullNameFilter(string _value)
        {
            Values = _value.Trim().Split(' ');
        }


        public override IEnumerable<ApplicationUser> Check(IEnumerable<ApplicationUser> _users)
        {
            IEnumerable<ApplicationUser> Users = _users.Where(us => Values.Any(val => us.FullName.ToLower().Contains(val.ToLower())));

            if(NextChain!=null)
            {
                return NextChain.Check(Users);
            }

            return Users;
        }
    }




}