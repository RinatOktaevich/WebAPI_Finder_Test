using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI_Finder_Test.Models;

namespace WebAPI_Finder_Test.Providers
{
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

        public abstract IEnumerable<ApplicationUser> Check(IEnumerable<ApplicationUser> users);

    }

    public class CityFilter : Filters
    {
        int value;

        public override IEnumerable<ApplicationUser> Check(IEnumerable<ApplicationUser> _users)
        {

            IEnumerable<ApplicationUser> users = _users.Where(xr => xr.CityId == value);

            //преходим к следующему звену
            if (NextChain != null)
            {
                return NextChain.Check(users);

            }



            return users;
        }
    }





}