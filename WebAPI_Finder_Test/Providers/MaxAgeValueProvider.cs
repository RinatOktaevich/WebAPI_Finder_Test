using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.ValueProviders;

namespace WebAPI_Finder_Test.Providers
{
    public class MaxAgeValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            return String.Compare("maxAge", prefix, true) == 0;
        }

        public ValueProviderResult GetValue(string key)
        {
            var maxAge = HttpContext.Current.Request.QueryString["maxAge"];
            if(String.IsNullOrEmpty(maxAge))
            {
                return null;
            }

            return ContainsPrefix(key) ? new ValueProviderResult(maxAge, null,
            CultureInfo.InvariantCulture) : null;
        }
    }
}