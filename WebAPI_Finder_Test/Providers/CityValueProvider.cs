using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.ValueProviders;

namespace WebAPI_Finder_Test.Providers
{
    public class CityValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            return string.Compare("cityid", prefix, true) == 0;
        }

        public ValueProviderResult GetValue(string key)
        {
            var cityid = HttpContext.Current.Request.QueryString["cityid"];
            if (String.IsNullOrEmpty(cityid))
            {
                return null;
            }

            return ContainsPrefix(key) ? new ValueProviderResult(cityid, null,
            CultureInfo.InvariantCulture) : null;
        }
    }
}