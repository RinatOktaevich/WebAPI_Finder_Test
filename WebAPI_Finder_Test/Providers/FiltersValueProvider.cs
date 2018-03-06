using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ValueProviders;

namespace WebAPI_Finder_Test.Providers
{
    public class FiltersValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            return string.Compare("filters",prefix,true) == 0;
        }

        public ValueProviderResult GetValue(string key)
        {
            var Request = HttpContext.Current.Request;
            if (ContainsPrefix(key))
            {
              //Request.QueryString[]
            }

            return null;
        }
    }
}