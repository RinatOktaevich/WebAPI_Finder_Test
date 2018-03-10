using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.ValueProviders;

namespace WebAPI_Finder_Test.Providers
{
    public class MinAgeValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            return string.Compare("minAge", prefix, true) == 0;
        }

        public ValueProviderResult GetValue(string key)
        {
            var minAge = HttpContext.Current.Request.QueryString["minAge"];
            if (minAge == string.Empty)
            {
                return null;
            }

            return ContainsPrefix(key) ? new ValueProviderResult(minAge, null, CultureInfo.InvariantCulture) : null;
        }
    }
}